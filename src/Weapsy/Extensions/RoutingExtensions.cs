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

        public async Task RouteAsync(RouteContext context)
        {
            // under development/refactoring
            Guid? pageId = null;
            Guid? languageId = null;
            string path = context.HttpContext.Request.Path.Value;

            if (!string.IsNullOrEmpty(path) && path[0] == '/')
                path = path.Substring(1);

            //if (path == string.Empty)

            var contextService = context.HttpContext.RequestServices.GetService<IContextService>();
            var languageRepository = context.HttpContext.RequestServices.GetService<ILanguageRepository>();
            var pageRepository = context.HttpContext.RequestServices.GetService<IPageRepository>();

            var pathParts = path.Split('/');
            var languagePart = pathParts.Length > 1 ? pathParts[0] : path;

            var siteId = contextService.GetCurrentContextInfo().Site.Id;

            
            languageId = languageRepository.GetIdBySlug(siteId, languagePart);

            if (languageId != null)
                path = languagePart == path ? string.Empty : path.Substring(languagePart.Length + 1);

            if (languageId != null)
                pageId = pageRepository.GetIdBySlug(siteId, path, languageId.Value);

            if (pageId == null)
                pageId = pageRepository.GetIdBySlug(siteId, path);

            if (pageId == null && languageId == null)
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
    }
}
