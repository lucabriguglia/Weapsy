using AutoMapper;
using Weapsy.Domain.Pages;
using Weapsy.Domain.Pages.Commands;
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
            CreateMap<PageModuleAdminModel, UpdatePageModuleDetails>().ForMember(x => x.PageModulePermissions, opt => opt.Ignore());
        }
    }
}