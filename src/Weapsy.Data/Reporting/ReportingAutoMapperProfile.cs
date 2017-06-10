using AutoMapper;
using Weapsy.Data.Entities;
using Weapsy.Reporting.Apps;
using Weapsy.Reporting.EmailAccounts;
using Weapsy.Reporting.Languages;
using Weapsy.Reporting.Menus;
using Weapsy.Reporting.Pages;
using Weapsy.Reporting.Sites;
using Weapsy.Reporting.Themes;
using Weapsy.Reporting.Users;

namespace Weapsy.Data.Reporting
{
    public class ReportingAutoMapperProfile : Profile
    {
        public ReportingAutoMapperProfile()
        {
            CreateMap<App, AppAdminModel>();
            CreateMap<App, AppAdminListModel>();
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
            CreateMap<Theme, ThemeAdminModel>();
            CreateMap<Theme, ThemeInfo>();
            CreateMap<User, UserAdminModel>();
            CreateMap<User, UserAdminListModel>();
            CreateMap<User, UserInfo>();
        }
    }
}