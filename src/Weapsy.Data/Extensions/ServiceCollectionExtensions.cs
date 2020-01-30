using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var dataProvider = configuration.GetSection("Weapsy:Data")["Provider"];
            var connectionString = configuration.GetConnectionString("WeapsyConnection");

            services.Configure<DataOptions>(options =>
            {
                options.Provider = dataProvider;
                options.ConnectionString = connectionString;
            });

            //var serviceProvider = services.BuildServiceProvider();
            //var dataOptions = serviceProvider.GetService<IOptions<DataOptions>>().Value;

            services.Scan(s => s
                .FromAssembliesOf(typeof(ServiceCollectionExtensions))
                .AddClasses()
                .AsImplementedInterfaces());

            var provider = services.BuildServiceProvider().GetServices<IDatabaseProvider>().Single(x => x.Provider.ToString() == dataProvider);
            provider.RegisterDbContext(services, connectionString);

            return services;
        }
    }
}
