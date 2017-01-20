using AutoMapper;

namespace Weapsy.Domain.Data.Tests
{
    public static class Shared
    {
        public static IMapper CreateNewMapper()
        {
            var autoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainAutoMapperProfile());
            });

            return autoMapperConfig.CreateMapper();
        }
    }
}
