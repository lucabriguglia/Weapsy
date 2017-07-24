using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.CodeAnalysis;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.ViewEngine;
using Weapsy.Services;
using Weapsy.Reporting.Languages;
using Weapsy.Domain.Sites;
using Weapsy.Extensions;
using Weapsy.Services.Installation;
using Weapsy.Mvc.Apps;
using Autofac.Core;
using Microsoft.Extensions.FileProviders;
using Weapsy.Data.Extensions;
using Weapsy.Reporting.Languages.Queries;
using Weapsy.Domain.Themes.Commands;
using Weapsy.Data.Configuration;
using Weapsy.Framework.Extensions;
using Weapsy.Framework.Queries;
using Weapsy.Mvc.Extensions;
using Weapsy.Reporting.Themes;
using Weapsy.Reporting.Themes.Queries;
using Microsoft.Extensions.Options;

namespace Weapsy
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment()) {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();

                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddTransient<IContextService, ContextService>();

            var hostingEnvironment = services.BuildServiceProvider().GetService<IHostingEnvironment>();

            services.AddOptions();

            services.Configure<Weapsy.Data.Configuration.Data>(c => {
                c.Provider = (Data.Configuration.DataProvider)Enum.Parse(
                    typeof(Data.Configuration.DataProvider), 
                    Configuration.GetSection("Data")["Provider"]);
            });

            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));

            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddEntityFramework(Configuration);

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization()
                .AddJsonOptions(options => {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                })
                .AddRazorOptions(options => {
                    foreach (var assembly in AppLoader.Instance(hostingEnvironment).AppAssemblies) {
                        var reference = MetadataReference.CreateFromFile(assembly.Location);
                        options.AdditionalCompilationReferences.Add(reference);
                    }
                });

            services.Configure<RazorViewEngineOptions>(options => {
                foreach (var assembly in AppLoader.Instance(hostingEnvironment).AppAssemblies) {
                    var embeddedFileProvider = new EmbeddedFileProvider(assembly, assembly.GetName().Name);
                    options.FileProviders.Add(embeddedFileProvider);
                }
                options.ViewLocationExpanders.Add(new ViewLocationExpander());
            });

            services.AddAutoMapper();

            foreach (var startup in AppLoader.Instance(hostingEnvironment).AppAssemblies.GetImplementationsOf<Mvc.Apps.IStartup>()) {
                startup.ConfigureServices(services);
            }

            var builder = new ContainerBuilder();

            foreach (var module in AppLoader.Instance(hostingEnvironment).AppAssemblies.GetImplementationsOf<IModule>()) {
                builder.RegisterModule(module);
            }

            builder.RegisterModule(new AutofacModule());
            builder.Populate(services);

            var container = builder.Build();

            return container.Resolve<IServiceProvider>();
        }

        public void Configure(IApplicationBuilder app,
            IHostingEnvironment hostingEnvironment,
            ILoggerFactory loggerFactory,
            ISiteInstallationService siteInstallationService,
            IThemeInstallationService themeInstallationService,
            ISiteRepository siteRepository,
            IQueryDispatcher queryDispatcher)
        {
            app.EnsureDbCreated();
            app.EnsureIdentityCreatedAsync();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseStatusCodePagesWithRedirects("~/error/{0}");

            if (hostingEnvironment.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else {
                app.UseExceptionHandler("/error/500");
            }

            app.UseTheme();

            app.UseStaticFiles();

            foreach (var theme in queryDispatcher.DispatchAsync<GetActiveThemes, IEnumerable<ThemeInfo>>(new GetActiveThemes()).Result) {
                var contentPath = Path.Combine(hostingEnvironment.ContentRootPath, "Themes", theme.Folder, "wwwroot");
                if (Directory.Exists(contentPath)) {
                    app.UseStaticFiles(new StaticFileOptions {
                        RequestPath = "/Themes/" + theme.Folder,
                        FileProvider = new PhysicalFileProvider(contentPath)
                    });
                }
            }

            foreach (var appDescriptor in AppLoader.Instance(hostingEnvironment).AppDescriptors) {
                var contentPath = Path.Combine(hostingEnvironment.ContentRootPath, "Apps", appDescriptor.Folder, "wwwroot");
                if (Directory.Exists(contentPath)) {
                    app.UseStaticFiles(new StaticFileOptions {
                        RequestPath = "/" + appDescriptor.Folder,
                        FileProvider = new PhysicalFileProvider(contentPath)
                    });
                }
            }

            foreach (var startup in AppLoader.Instance(hostingEnvironment).AppAssemblies.GetImplementationsOf<Mvc.Apps.IStartup>()) {
                startup.Configure(app);
            }

            var applicationPartManager = app.ApplicationServices.GetRequiredService<ApplicationPartManager>();
            Parallel.ForEach(AppLoader.Instance(hostingEnvironment).AppAssemblies, assembly => {
                applicationPartManager.ApplicationParts.Add(new AssemblyPart(assembly));
            });

            app.UseIdentity();

            themeInstallationService.EnsureThemeInstalled(new CreateTheme { Name = "Default", Description = "Default Theme", Folder = "Default" });
            siteInstallationService.VerifySiteInstallation();

            var site = siteRepository.GetByName("Default");
            var activeLanguages = queryDispatcher.DispatchAsync<GetAllActive, IEnumerable<LanguageInfo>>(new GetAllActive { SiteId = site.Id }).Result;

            app.AddRoutes();
            app.AddLocalisation(activeLanguages);
        }
    }
}
