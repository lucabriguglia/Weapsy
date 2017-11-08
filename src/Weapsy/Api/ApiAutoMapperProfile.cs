using AutoMapper;
using Weapsy.Domain.Pages;
using Weapsy.Domain.Pages.Commands;
using Weapsy.Domain.Sites;
using Weapsy.Domain.Sites.Commands;
using Weapsy.Reporting.Pages;
using Weapsy.Reporting.Sites;

namespace Weapsy.Api
{
    public class ApiAutoMapperProfile : Profile
    {
        public ApiAutoMapperProfile()
        {
            CreateMap<PageAdminModel, CreatePageCommand>().ForMember(x => x.PagePermissions, opt => opt.Ignore());
            CreateMap<PageLocalisationAdminModel, PageLocalisation>();
            CreateMap<PageAdminModel, UpdatePageDetailsCommand>().ForMember(x => x.PagePermissions, opt => opt.Ignore());
            CreateMap<PageModuleAdminModel, UpdatePageModuleDetailsCommand>().ForMember(x => x.PageModulePermissions, opt => opt.Ignore());
            CreateMap<SiteAdminModel, UpdateSiteDetailsCommand>();
            CreateMap<SiteLocalisationAdminModel, SiteLocalisation>();
        }
    }
}