using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Weapsy.Domain.Data;

namespace Weapsy.Extensions
{
    public static class AutoMapperExtensions
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            var autoMapperConfig = new MapperConfiguration(cfg =>
            {
                // temporary: all profiles will be added automatically 
                cfg.AddProfile(new Api.AutoMapperProfile());
                cfg.AddProfile(new AutoMapperProfile());
                cfg.AddProfile(new Reporting.Data.AutoMapperProfile());
                cfg.AddProfile(new Apps.Text.Data.AutoMapperProfile());
            });

            services.AddSingleton(sp => autoMapperConfig.CreateMapper());

            return services;
        }
    }
}
