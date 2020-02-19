using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Weapsy.Data.Extensions;

namespace Weapsy.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWeapsy(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            services.AddWeapsyData(configuration);

            return services;
        }
    }
}
