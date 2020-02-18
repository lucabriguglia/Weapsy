using AutoMapper;
using Weapsy.Data.Entities;
using Weapsy.Domain.Models.Sites;

namespace Weapsy.Domain.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Site, SiteEntity>();
            CreateMap<SiteEntity, Site>().ConstructUsing(x => new Site());
        }
    }
}
