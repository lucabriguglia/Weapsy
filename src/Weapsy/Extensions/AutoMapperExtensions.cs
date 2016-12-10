using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Weapsy.Domain.Data;
using Weapsy.Mvc.Apps;

namespace Weapsy.Extensions
{
    public static class AutoMapperExtensions
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services, IHostingEnvironment hostingEnvironment)
        {
            var autoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new Api.AutoMapperProfile());
                cfg.AddProfile(new AutoMapperProfile());
                cfg.AddProfile(new Reporting.Data.AutoMapperProfile());

                foreach (var profile in AppLoader.Instance(hostingEnvironment).AppAssemblies.GetTypes<Profile>())
                {
                    cfg.AddProfile(profile);
                }
            });

            services.AddSingleton(sp => autoMapperConfig.CreateMapper());

            return services;
        }
    }
}
