using AutoMapper;
using Weapsy.Data.Entities;
using Weapsy.Reporting.Models.Sites;

namespace Weapsy.Reporting.Handlers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SiteInfo, SiteEntity>();
            CreateMap<SiteEntity, SiteInfo>();
        }
    }
}
