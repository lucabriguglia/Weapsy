#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Weapsy.Cqrs;
using Weapsy.Cqrs.EventStore.EF;
using Weapsy.Cqrs.EventStore.EF.SqlServer;
using Weapsy.Cqrs.Extensions;
using Weapsy.Data;
using Weapsy.Data.Configuration;
using Weapsy.Data.Extensions;
using Weapsy.Data.TempIdentity;
using Weapsy.Domain.Sites;
using Weapsy.Domain.Themes.Commands;
using Weapsy.Mvc.Apps;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Extensions;
using Weapsy.Mvc.ViewEngine;
using Weapsy.Reporting.Languages;
using Weapsy.Reporting.Languages.Queries;
using Weapsy.Reporting.Themes;
using Weapsy.Reporting.Themes.Queries;
using Weapsy.Services.Installation;
using Weapsy.Web.Extensions;
using Weapsy.Web.Services;

#endregion

namespace Weapsy.Web
{
    // TODO: Mess to be cleaned up. Everything will be moved to Weapsy.Mvc project.
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddTransient<IContextService, ContextService>();

            var hostingEnvironment = services.BuildServiceProvider().GetService<IHostingEnvironment>();

            services.AddOptions();

            services.AddWeapsyCqrs();
            services.AddWeapsyCqrsEventStore(Configuration);

            services.Configure<Weapsy.Data.Configuration.Data>(c => {
                c.Provider = (Data.Configuration.DataProvider)Enum.Parse(
                    typeof(Data.Configuration.DataProvider), 
                    Configuration.GetSection("Data")["Provider"]);
            });

            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));

            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddEntityFramework(Configuration);

            // TEMP
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // TEMP
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

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

            foreach (var startup in AppLoader.Instance(hostingEnvironment).AppAssemblies.GetImplementationsOf<Mvc.Apps.IStartup>())
            {
                startup.ConfigureServices(services, Configuration);
            }

            var builder = new ContainerBuilder();

            foreach (var module in AppLoader.Instance(hostingEnvironment).AppAssemblies.GetImplementationsOf<IModule>())
            {
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
            IDispatcher dispatcher,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            ApplicationDbContext applicationDbCopntext,
            WeapsyDbContext weapsyDbContext, 
            EventStoreDbContext eventStoreDbContext)
        {
            weapsyDbContext.Database.Migrate();
            applicationDbCopntext.Database.Migrate();
            eventStoreDbContext.Database.Migrate();

            //app.EnsureDbCreated();
            app.EnsureIdentityCreatedAsync(userManager, roleManager);

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseStatusCodePagesWithRedirects("~/error/{0}");

            if (hostingEnvironment.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/error/500");
            }

            app.UseTheme();

            app.UseStaticFiles();

            foreach (var theme in dispatcher.GetResultAsync<GetActiveThemes, IEnumerable<ThemeInfo>>(new GetActiveThemes()).GetAwaiter().GetResult()) {
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

            app.UseAuthentication();

            themeInstallationService.EnsureThemeInstalled(new CreateTheme { Name = "Default", Description = "Default Theme", Folder = "Default" });
            siteInstallationService.VerifySiteInstallation();

            var site = siteRepository.GetByName("Default");
            var activeLanguages = dispatcher.GetResultAsync<GetAllActive, IEnumerable<LanguageInfo>>(new GetAllActive { SiteId = site.Id }).GetAwaiter().GetResult();

            app.AddRoutes();
            app.AddLocalisation(activeLanguages);
        }
    }
}
