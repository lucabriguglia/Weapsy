using AutoMapper;
using Weapsy.Data.Entities;
using Weapsy.Reporting.Models.Sites;

namespace Weapsy.Reporting.Handlers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SiteModel, SiteEntity>();
            CreateMap<SiteEntity, SiteModel>();
        }
    }
}
