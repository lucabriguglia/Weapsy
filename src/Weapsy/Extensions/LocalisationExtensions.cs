using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Weapsy.Reporting.Languages;
using Microsoft.AspNetCore.Localization;

namespace Weapsy.Extensions
{
    public static class LocalisationExtensions
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
    }
}
