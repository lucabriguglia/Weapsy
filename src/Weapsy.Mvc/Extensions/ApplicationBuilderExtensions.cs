using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Weapsy.Mvc.Middleware;
using Weapsy.Reporting.Languages;

namespace Weapsy.Mvc.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder AddLocalisation(this IApplicationBuilder app, 
            IEnumerable<LanguageInfo> languages)
        {
            var supportedCultures = new List<CultureInfo>();
            RequestCulture defaultRequestCulture;

            try
            {
                foreach (var language in languages)
                    supportedCultures.Add(new CultureInfo(language.CultureName));

                defaultRequestCulture = new RequestCulture(supportedCultures[0].Name);
            }
            catch (Exception)
            {
                supportedCultures.Add(new CultureInfo("en"));
                defaultRequestCulture = new RequestCulture("en");
            }

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = defaultRequestCulture,
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            return app;
        }

        public static IApplicationBuilder AddRoutes(this IApplicationBuilder app)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "area",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.Routes.Add(new PageSlugRoute(routes.DefaultHandler));

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            return app;
        }

        public static IApplicationBuilder UseTheme(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ThemeMiddleware>();
        }
    }
}
