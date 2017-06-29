using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weapsy.Apps.Text.Data;
using Weapsy.Framework.Extensions;

namespace Weapsy.Apps.Text.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityFramework(this IServiceCollection services, IConfigurationRoot configuration)
        {
            var dataProviderConfig = configuration.GetSection("Data")["Provider"];
            var connectionStringConfig = configuration.GetConnectionString("DefaultConnection");

            var currentAssembly = typeof(ServiceCollectionExtensions).GetTypeInfo().Assembly;
            var dataProviders = currentAssembly.GetImplementationsOf<IDataProvider>();

            var dataProvider = dataProviders.SingleOrDefault(x => x.Provider.ToString() == dataProviderConfig);

            dataProvider.RegisterDbContext(services, connectionStringConfig);

            return services;
        }
    }
}