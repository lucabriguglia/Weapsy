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
            context.Request.HttpContext.Items[ViewLocationExpander.ThemeKey] = _contextService.GetCurrentThemeInfo().Folder;
            return _next(context);
        }
    }
}
