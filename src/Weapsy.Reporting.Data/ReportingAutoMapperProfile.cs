using AutoMapper;
using Weapsy.Data.Entities;
using Weapsy.Reporting.Apps;
using Weapsy.Reporting.EmailAccounts;
using Weapsy.Reporting.Languages;
using Weapsy.Reporting.Menus;
using Weapsy.Reporting.ModuleTypes;
using Weapsy.Reporting.Pages;
using Weapsy.Reporting.Sites;

namespace Weapsy.Reporting.Data
{
    public class ReportingAutoMapperProfile : Profile
    {
        public ReportingAutoMapperProfile()
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