using AutoMapper;
using Weapsy.Domain.Apps;
using Weapsy.Domain.EmailAccounts;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Menus;
using Weapsy.Domain.ModuleTypes;
using Weapsy.Domain.Pages;
using Weapsy.Domain.Sites;
using Weapsy.Reporting.Apps;
using Weapsy.Reporting.EmailAccounts;
using Weapsy.Reporting.Languages;
using Weapsy.Reporting.Menus;
using Weapsy.Reporting.ModuleTypes;
using Weapsy.Reporting.Pages;
using Weapsy.Reporting.Sites;

namespace Weapsy.Reporting.Data.Default
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