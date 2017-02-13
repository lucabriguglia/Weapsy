using System.Linq;
using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weapsy.Data.Entities;
using Weapsy.Infrastructure.Extensions;

namespace Weapsy.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityFramework(this IServiceCollection services, IConfigurationRoot configuration)
        {
            var dataProviderConfig = configuration.GetSection("Data")["Provider"];
            var connectionStringConfig = configuration.GetConnectionString("DefaultConnection");

            var currentAssembly = typeof(ServiceCollectionExtensions).GetTypeInfo().Assembly;
            var dataProviders = currentAssembly.GetTypes<IDataProvider>();

            var dataProvider = dataProviders.SingleOrDefault(x => x.Provider.ToString() == dataProviderConfig);

            dataProvider.RegisterDbContext(services, connectionStringConfig);

            //services.AddIdentity<User, IdentityRole>()
            //    .AddEntityFrameworkStores<WeapsyDbContext>()
            //    .AddDefaultTokenProviders();

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
    }
}
