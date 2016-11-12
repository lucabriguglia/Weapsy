using AutoMapper;

namespace Weapsy.Reporting.Data.Default.Tests
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
