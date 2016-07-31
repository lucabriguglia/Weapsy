using AutoMapper;
using Weapsy.Domain.Model.Pages;
using Weapsy.Domain.Model.Pages.Commands;
using Weapsy.Reporting.Pages;

namespace Weapsy.Api
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<PageAdminModel, CreatePage>().ForMember(x => x.PagePermissions, opt => opt.Ignore());
            CreateMap<PageLocalisationAdminModel, PageLocalisation>();
            CreateMap<PageAdminModel, UpdatePageDetails>().ForMember(x => x.PagePermissions, opt => opt.Ignore());
        }
    }
}