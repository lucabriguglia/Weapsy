using AutoMapper;
using Weapsy.Data.Entities;
using Weapsy.Reporting.Apps;
using Weapsy.Reporting.EmailAccounts;
using Weapsy.Reporting.Languages;
using Weapsy.Reporting.Menus;
using Weapsy.Reporting.ModuleTypes;
using Weapsy.Reporting.Pages;
using Weapsy.Reporting.Sites;
using Menu = Weapsy.Domain.Menus.Menu;
using ModuleType = Weapsy.Domain.ModuleTypes.ModuleType;
using Page = Weapsy.Domain.Pages.Page;
using PageLocalisation = Weapsy.Domain.Pages.PageLocalisation;
using Site = Weapsy.Domain.Sites.Site;
using SiteLocalisation = Weapsy.Domain.Sites.SiteLocalisation;

namespace Weapsy.Reporting.Data
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<App, AppAdminModel>();
            CreateMap<App, AppAdminListModel>();
            CreateMap<EmailAccount, EmailAccountModel>();
            CreateMap<EmailAccount, EmailAccountModel>();
            CreateMap<Language, LanguageInfo>();
            CreateMap<Language, LanguageAdminModel>();
            CreateMap<Menu, MenuAdminModel>();
            CreateMap<ModuleType, ModuleTypeAdminModel>();
            CreateMap<ModuleType, ModuleTypeAdminListModel>();
            CreateMap<Page, PageAdminModel>();
            CreateMap<Page, PageAdminListModel>();
            CreateMap<PageLocalisation, PageLocalisationAdminListModel>();
            CreateMap<Site, SiteAdminModel>();
            CreateMap<SiteLocalisation, SiteLocalisationAdminModel>();
            CreateMap<Site, SiteInfo>();
        }
    }
}