using AutoMapper;

namespace Weapsy.Reporting.Data.Tests
{
    public static class Shared
    {
        public static IMapper CreateNewMapper()
        {
            var autoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ReportingAutoMapperProfile());
            });

            return autoMapperConfig.CreateMapper();
        }
    }
}
