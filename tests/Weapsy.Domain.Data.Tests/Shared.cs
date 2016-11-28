using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Weapsy.Data;

namespace Weapsy.Domain.Data.Tests
{
    public static class Shared
    {
        public static IMapper CreateNewMapper()
        {
            var autoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            return autoMapperConfig.CreateMapper();
        }
    }
}
