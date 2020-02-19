using AutoMapper;
using Weapsy.Domain.Models.Sites;
using Weapsy.Reporting.Sites.Models;

namespace Weapsy.Reporting
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SiteInfo, Site>();
            CreateMap<Site, SiteInfo>();
        }
    }
}
