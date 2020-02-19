using AutoMapper;
using Weapsy.Domain.Models.Sites;
using Weapsy.Reporting.Models.Sites;

namespace Weapsy.Reporting.Handlers
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
