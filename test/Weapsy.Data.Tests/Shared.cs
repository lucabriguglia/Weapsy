using AutoMapper;
using Weapsy.Data.Reporting;
using Weapsy.Data.Repositories;

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
