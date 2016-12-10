using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Weapsy.Api;
using Weapsy.Domain.Data;
using Weapsy.Mvc.Apps;
using Weapsy.Reporting.Data;

namespace Weapsy.Extensions
{
    public static class AutoMapperExtensions
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services, IHostingEnvironment hostingEnvironment)
        {
            var autoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApiAutoMapperProfile());
                cfg.AddProfile(new DomainAutoMapperProfile());
                cfg.AddProfile(new ReportingAutoMapperProfile());

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
