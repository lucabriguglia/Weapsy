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
                cfg.AddProfile(new Reporting.Data.Default.AutoMapperProfile());
                cfg.AddProfile(new Apps.Text.Data.SqlServer.AutoMapperProfile());
            });

            services.AddSingleton(sp => autoMapperConfig.CreateMapper());

            return services;
        }
    }
}
