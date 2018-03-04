using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Weapsy.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
        {
            var dataProviderConfig = configuration.GetSection("Data")["Provider"];
            var connectionStringConfig = configuration.GetConnectionString("DefaultConnection");

            var currentAssembly = typeof(ServiceCollectionExtensions).GetTypeInfo().Assembly;
            var dataProviders = currentAssembly.GetImplementationsOf<IDataProvider>();

            var dataProvider = dataProviders.SingleOrDefault(x => x.Provider.ToString() == dataProviderConfig);

            dataProvider.RegisterDbContext(services, connectionStringConfig);

            return services;
        }

        //public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        //{
        //    var autoMapperConfig = new MapperConfiguration(cfg =>
        //    {
        //        var currentAssembly = typeof(ServiceCollectionExtensions).GetTypeInfo().Assembly;
        //        var profiles = currentAssembly.GetTypes<Profile>();

        //        foreach (var profile in profiles)
        //        {
        //            cfg.AddProfile(profile);
        //        }
        //    });

        //    services.AddSingleton(sp => autoMapperConfig.CreateMapper());

        //    return services;
        //}

        private static IEnumerable<T> GetImplementationsOf<T>(this Assembly assembly)
        {
            var result = new List<T>();

            var types = assembly.GetTypes()
                .Where(t => t.GetTypeInfo().IsClass && !t.GetTypeInfo().IsAbstract && typeof(T).IsAssignableFrom(t))
                .ToList();

            foreach (var type in types)
            {
                var instance = (T)Activator.CreateInstance(type);
                result.Add(instance);
            }

            return result;
        }
    }
}
