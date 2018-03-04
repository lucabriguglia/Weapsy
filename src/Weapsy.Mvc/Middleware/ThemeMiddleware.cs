using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.ViewEngine;

namespace Weapsy.Mvc.Middleware
{
    public class ThemeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IContextService _contextService;

        public ThemeMiddleware(RequestDelegate next,
            IContextService contextService)
        {
            _next = next;
            _contextService = contextService;
        }

        public Task Invoke(HttpContext context)
        {
            var folder = _contextService.GetCurrentThemeInfo()?.Folder;
            context.Request.HttpContext.Items[ViewLocationExpander.ThemeKey] = folder ?? "Default";
            return _next(context);
        }
    }
}
