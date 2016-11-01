using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Weapsy.Mvc.Context;
using Weapsy.Reporting.Languages;
using Weapsy.Reporting.Pages;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Weapsy.Domain.Sites;

namespace Weapsy.Extensions
{
    public static class RoutingExtensions
    {
        public static IApplicationBuilder AddRoutes(this IApplicationBuilder app,
            Site site,
            IEnumerable<LanguageInfo> languages, 
            IEnumerable<PageAdminListModel> pages)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "area",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                if (site != null)
                {
                    foreach (var page in pages)
                    {
                        var defaults = new RouteValueDictionary();
                        var constraints = new RouteValueDictionary();
                        var tokens = new RouteValueDictionary();

                        defaults.Add("controller", "Home");
                        defaults.Add("action", "Index");
                        defaults.Add("data", string.Empty);

                        constraints.Add("data", @"[a-zA-Z0-9\-]*");

                        tokens.Add("namespaces", new[] { "Weapsy.Controllers" });
                        tokens.Add(ContextKeys.PageKey, page.Id);
                        //tokens.Add("languageId", default language id);

                        routes.MapRoute(
                            name: page.Name,
                            template: page.Url,
                            defaults: defaults,
                            constraints: constraints,
                            dataTokens: tokens);

                        foreach (var language in languages)
                        {
                            if (defaults.ContainsKey("culture"))
                                defaults.Remove("culture");

                            defaults.Add("culture", language.CultureName);

                            //if (tokens.ContainsKey(ContextKeys.LanguageKey))
                            //    tokens.Remove(ContextKeys.LanguageKey);

                            //tokens.Add(ContextKeys.LanguageKey, language.Id);                            

                            var pageLocalisation = page.PageLocalisations.FirstOrDefault(x => x.LanguageId == language.Id);

                            var url = !string.IsNullOrWhiteSpace(pageLocalisation?.Url)
                                ? pageLocalisation.Url
                                : page.Url;

                            routes.MapRoute(
                                name: $"{page.Name} - {language.Name}",
                                template: $"{language.Url}/{url}",
                                defaults: defaults,
                                constraints: constraints,
                                dataTokens: tokens);
                        }
                    }
                }

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            return app;
        }
    }
}
