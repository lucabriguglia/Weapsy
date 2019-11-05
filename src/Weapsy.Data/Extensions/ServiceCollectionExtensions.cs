using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Weapsy.Data.Configuration;

namespace Weapsy.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWeapsyData(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            services.Configure<DataConfiguration>(configuration.GetSection("Data"));

            services.Scan(s => s
                .FromAssembliesOf(typeof(ServiceCollectionExtensions))
                .AddClasses()
                .AsImplementedInterfaces());

            var dataProvider = configuration.GetSection("Data")["Provider"];
            var connectionString= configuration.GetConnectionString("DefaultConnection");

            var provider = services.BuildServiceProvider().GetServices<IDatabaseProvider>().Single(x => x.Provider.ToString() == dataProvider);
            provider.RegisterDbContext(services, connectionString);

            return services;
        }
    }
}
