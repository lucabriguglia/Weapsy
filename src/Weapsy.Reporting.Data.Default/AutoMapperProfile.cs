using AutoMapper;
using Weapsy.Domain.Model.Apps;
using Weapsy.Domain.Model.Languages;
using Weapsy.Domain.Model.Menus;
using Weapsy.Domain.Model.ModuleTypes;
using Weapsy.Domain.Model.Pages;
using Weapsy.Domain.Model.Sites;
using Weapsy.Reporting.Apps;
using Weapsy.Reporting.Languages;
using Weapsy.Reporting.Menus;
using Weapsy.Reporting.ModuleTypes;
using Weapsy.Reporting.Pages;
using Weapsy.Reporting.Sites;

namespace Weapsy.Reporting.Data
{
    public class AutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<App, AppAdminModel>();
            CreateMap<App, AppAdminListModel>();
            CreateMap<Language, LanguageViewModel>();
            CreateMap<Language, LanguageAdminModel>();
            CreateMap<Menu, MenuAdminModel>();
            CreateMap<ModuleType, ModuleTypeAdminModel>();
            CreateMap<ModuleType, ModuleTypeAdminListModel>();
            CreateMap<Page, PageAdminModel>();
            CreateMap<Page, PageAdminListModel>();
            CreateMap<Site, SiteAdminModel>();
            CreateMap<Site, SiteSettings>();
        }
    }
}