using Microsoft.AspNetCore.Builder;
using System;

namespace Weapsy.Web.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IWeapsyAppBuilder UseWeapsy(this IApplicationBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            //builder.EnsureDatabaseCreated();

            return new WeapsyAppBuilder(builder);
        }
    }
}