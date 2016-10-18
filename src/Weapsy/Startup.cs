using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using System;
using System.Globalization;
using Weapsy.Data;
using Weapsy.DependencyConfigurator;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.ViewEngine;
using Weapsy.Reporting.Pages;
using Weapsy.Services;
using System.Collections.Generic;
using Weapsy.Apps.Text;
using Weapsy.Core.Configuration;
using Weapsy.Reporting.Languages;
using Weapsy.Domain.Apps;
using Weapsy.Domain.Sites;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;
using Weapsy.Extensions;
using Weapsy.Services.Installation;

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
            // Add functionality to inject IOptions<T>
            services.AddOptions();

            // Add our Config object so it can be injected
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));

            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //services
            //    .AddScoped(p => new WeapsyDbContext(p.GetService<DbContextOptions<WeapsyDbContext>>()));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc()
                //.AddViewComponentsAsServices
                //.AddControllersAsServices
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix, options => options.ResourcesPath = "Resources")
                .AddDataAnnotationsLocalization()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new WeapsyViewLocationExpander());
            });

            services.AddAutoMapper();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddTransient<IContextService, ContextService>();

            var builder = new ContainerBuilder();

            // temporary: all modules will be added automatically 
            builder.RegisterModule(new WeapsyModule());
            builder.RegisterModule(new AutofacModule());
            builder.Populate(services);

            var container = builder.Build();

            // ===== Temporary ===== //
            _appInstallationService = container.Resolve<IAppInstallationService>();
            _siteInstallationService = container.Resolve<ISiteInstallationService>();
            _appRepository = container.Resolve<IAppRepository>();
            _siteRepository = container.Resolve<ISiteRepository>();
            _pageFacade = container.Resolve<IPageFacade>();
            _languageFacade = container.Resolve<ILanguageFacade>();
            _userManager = container.Resolve<UserManager<IdentityUser>>();
            _roleManager = container.Resolve<RoleManager<IdentityRole>>();
            // ==================== //

            return container.Resolve<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // ===== Temporary ===== //
            CheckIfDeafultAppsHaveBeenInstalled();
            CheckIfDeafultSiteHasBeenInstalled();
            CheckIfAdminUserHasBeenCreated();
            // ==================== //

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            app.UseStatusCodePagesWithRedirects("~/error/{0}");

            if (env.IsDevelopment())
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

            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            var site = _siteRepository.GetByName("Default");
            var activeLanguages = _languageFacade.GetAllActive(site.Id);
            var pages = _pageFacade.GetAllForAdminAsync(site.Id).Result;

            app.AddRoutes(site, activeLanguages, pages);
            app.AddLocalisation(activeLanguages);
        }

        private void CheckIfDeafultAppsHaveBeenInstalled()
        {
            if (_appRepository.GetByName("Text") == null)
            {
                _appInstallationService.InstallDefaultApps();
            }
        }

        private void CheckIfDeafultSiteHasBeenInstalled()
        {
            if (_siteRepository.GetByName("Default") == null)
            {
                _siteInstallationService.InstallDefaultSite();
            }
        }

        private async Task CheckIfAdminUserHasBeenCreated()
        {
            var adminEmail = "admin@default.com";
            var adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail };

            var adminRoleName = "Administrator";
            var adminRole = new IdentityRole(adminRoleName);

            if (await _userManager.FindByEmailAsync(adminEmail) == null)
            {
                await _userManager.CreateAsync(adminUser, "Ab1234567!");
            }

            if (!await _roleManager.RoleExistsAsync(adminRoleName))
            {
                await _roleManager.CreateAsync(adminRole);
            }

            adminUser = await _userManager.FindByEmailAsync(adminEmail);

            if (!await _userManager.IsInRoleAsync(adminUser, adminRoleName))
            {
                await _userManager.AddToRoleAsync(adminUser, adminRoleName);
            }
        }

        private static IAppInstallationService _appInstallationService;
        private static ISiteInstallationService _siteInstallationService;
        private static IAppRepository _appRepository;
        private static ISiteRepository _siteRepository;
        private static IPageFacade _pageFacade;
        private static ILanguageFacade _languageFacade;
        private static UserManager<IdentityUser> _userManager;
        private static RoleManager<IdentityRole> _roleManager;
    }
}
