using AutoMapper;
using Weapsy.Data.Domain;
using Weapsy.Data.Reporting;

namespace Weapsy.Data.Tests
{
    public static class Shared
    {
        public static IMapper CreateNewMapper()
        {
            var autoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainAutoMapperProfile());
                cfg.AddProfile(new ReportingAutoMapperProfile());
            });

            return autoMapperConfig.CreateMapper();
        }
    }
}
