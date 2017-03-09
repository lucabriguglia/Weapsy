using Microsoft.AspNetCore.Builder;
using Weapsy.Mvc.Middleware;

namespace Weapsy.Mvc.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseTheme(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ThemeMiddleware>();
        }
    }
}
