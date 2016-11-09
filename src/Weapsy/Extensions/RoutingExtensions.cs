using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Weapsy.Mvc.Context;
using Weapsy.Reporting.Languages;
using Weapsy.Reporting.Pages;
using System.Threading.Tasks;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Pages;
using Weapsy.Domain.Sites;
using System.Linq;

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

                routes.Routes.Add(new PageSlugRoute(routes.DefaultHandler));

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            return app;
        }
    }

    public class PageSlugRoute : IRouter
    {
        private readonly IRouter _router;

        public PageSlugRoute(IRouter router)
        {
            _router = router;
        }

        // under development/refactoring
        public async Task RouteAsync(RouteContext context)
        {
            string path = GetPath(context);

            var contextService = context.HttpContext.RequestServices.GetService<IContextService>();
            var languageRepository = context.HttpContext.RequestServices.GetService<ILanguageRepository>();
            var pageRepository = context.HttpContext.RequestServices.GetService<IPageRepository>();

            var pathParts = path.Split('/');
            var languageSlug = pathParts.Length > 1 ? pathParts[0] : path;
            string pageSlug;

            var siteId = contextService.GetCurrentContextInfo().Site.Id;
            Guid? languageId = languageRepository.GetIdBySlug(siteId, languageSlug);
            Guid? pageId = null;

            if (languageId != null)
            {
                pageSlug = languageSlug == path ? string.Empty : path.Substring(languageSlug.Length + 1);

                if (!string.IsNullOrEmpty(pageSlug))
                    pageId = pageRepository.GetIdBySlug(siteId, pageSlug, languageId.Value);
            }
            else
            {
                pageSlug = path;
            }
            
            if (pageId == null && !string.IsNullOrEmpty(pageSlug))
                pageId = pageRepository.GetIdBySlug(siteId, pageSlug);

            if (pageId == null && string.IsNullOrEmpty(pageSlug))
            {
                // pageId = site.HomePageId // todo: set pageId to the home page of current site
                var pages = pageRepository.GetAll(siteId);
                var homePage = pages.FirstOrDefault(x => x.Name == "Home");
                if (homePage != null)
                    pageId = homePage.Id;
            }

            if (pageId == null)
                return;

            var routeData = new RouteData(context.RouteData);
            routeData.Routers.Add(_router);
            routeData.Values["controller"] = "Home";
            routeData.Values["action"] = "Index";
            routeData.Values[ContextKeys.PageKey] = pageId;
            routeData.Values[ContextKeys.LanguageKey] = languageId;

            context.RouteData = routeData;

            await _router.RouteAsync(context);
        }

        public VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            return null;
        }

        private string GetPath(RouteContext context)
        {
            string path = context.HttpContext.Request.Path.Value;

            if (!string.IsNullOrEmpty(path) && path[0] == '/')
                path = path.Substring(1);

            return path;
        }
    }
}
