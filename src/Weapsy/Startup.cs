using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.CodeAnalysis;
using Weapsy.Data;
using Weapsy.DependencyConfigurator;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.ViewEngine;
using Weapsy.Reporting.Pages;
using Weapsy.Services;
using Weapsy.Infrastructure.Configuration;
using Weapsy.Reporting.Languages;
using Weapsy.Domain.Sites;
using Weapsy.Extensions;
using Weapsy.Services.Installation;
using Weapsy.Mvc.Apps;
using Autofac.Core;
using Microsoft.Extensions.FileProviders;

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

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();

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

            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));

            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                })
                .AddRazorOptions(options =>
                {
                    foreach (var assembly in AppLoader.Instance(hostingEnvironment).AppAssemblies)
                    {
                        var reference = MetadataReference.CreateFromFile(assembly.Location);
                        options.AdditionalCompilationReferences.Add(reference);
                    }
                });

            services.Configure<RazorViewEngineOptions>(options =>
            {
                foreach (var assembly in AppLoader.Instance(hostingEnvironment).AppAssemblies)
                {
                    var embeddedFileProvider = new EmbeddedFileProvider(assembly, assembly.GetName().Name);
                    options.FileProviders.Add(embeddedFileProvider);
                }
                options.ViewLocationExpanders.Add(new ViewLocationExpander());
            });

            services.AddAutoMapper();

            foreach (var startup in AppLoader.Instance(hostingEnvironment).AppAssemblies.GetTypes<Mvc.Apps.IStartup>())
            {
                startup.ConfigureServices(services);
            }

            var builder = new ContainerBuilder();

            foreach (var module in AppLoader.Instance(hostingEnvironment).AppAssemblies.GetTypes<IModule>())
            {
                builder.RegisterModule(module);
            }

            builder.RegisterModule(new WeapsyModule());
            builder.Populate(services);

            var container = builder.Build();

            return container.Resolve<IServiceProvider>();
        }

        public void Configure(IApplicationBuilder app,
            IHostingEnvironment hostingEnvironment,
            ILoggerFactory loggerFactory,
            ISiteInstallationService siteInstallationService,
            IAppInstallationService appInstallationService,
            IMembershipInstallationService membershipInstallationService,
            ISiteRepository siteRepository,
            ILanguageFacade languageFacade,
            IPageFacade pageFacade)
        {
            membershipInstallationService.VerifyUserCreation();
            appInstallationService.VerifyAppInstallation();
            siteInstallationService.VerifySiteInstallation();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            app.UseStatusCodePagesWithRedirects("~/error/{0}");

            if (hostingEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/error/500");
            }

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();

            foreach (var appDescriptor in AppLoader.Instance(hostingEnvironment).AppDescriptors)
            {
                var contentPath = Path.Combine(hostingEnvironment.ContentRootPath, "Apps", appDescriptor.Folder, "wwwroot");
                if (Directory.Exists(contentPath))
                {
                    app.UseStaticFiles(new StaticFileOptions
                    {
                        RequestPath = "/" + appDescriptor.Folder,
                        FileProvider = new PhysicalFileProvider(contentPath)
                    });
                }
            }

            foreach (var startup in AppLoader.Instance(hostingEnvironment).AppAssemblies.GetTypes<Mvc.Apps.IStartup>())
            {
                startup.Configure(app);
            }

            var applicationPartManager = app.ApplicationServices.GetRequiredService<ApplicationPartManager>();
            Parallel.ForEach(AppLoader.Instance(hostingEnvironment).AppAssemblies, assembly =>
            {
                applicationPartManager.ApplicationParts.Add(new AssemblyPart(assembly));
            });

            app.UseIdentity();

            var site = siteRepository.GetByName("Default");
            var activeLanguages = languageFacade.GetAllActiveAsync(site.Id).Result;

            app.AddRoutes();
            app.AddLocalisation(activeLanguages);
        }
    }
}
