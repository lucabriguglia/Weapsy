using Microsoft.AspNetCore.Builder;

namespace Weapsy.Web.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IWeapsyAppBuilder UseWeapsy(this IApplicationBuilder app)
        {
            return new WeapsyAppBuilder(app);
        }
    }
}