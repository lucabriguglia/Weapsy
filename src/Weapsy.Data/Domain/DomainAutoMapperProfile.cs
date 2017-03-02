using AutoMapper;
using Weapsy.Domain.Apps;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Menus;
using Weapsy.Domain.Modules;
using Weapsy.Domain.ModuleTypes;
using Weapsy.Domain.Pages;
using Weapsy.Domain.Sites;
using Weapsy.Domain.Themes;
using Weapsy.Domain.Users;
using AppDbEntity = Weapsy.Data.Entities.App;
using LanguageDbEntity = Weapsy.Data.Entities.Language;
using MenuDbEntity = Weapsy.Data.Entities.Menu;
using MenuItemDbEntity = Weapsy.Data.Entities.MenuItem;
using MenuItemLocalisationDbEntity = Weapsy.Data.Entities.MenuItemLocalisation;
using MenuItemPermissionDbEntity = Weapsy.Data.Entities.MenuItemPermission;
using ModuleDbEntity = Weapsy.Data.Entities.Module;
using ModuleTypeDbEntity = Weapsy.Data.Entities.ModuleType;
using PageDbEntity = Weapsy.Data.Entities.Page;
using PageLocalisationDbEntity = Weapsy.Data.Entities.PageLocalisation;
using PageModuleDbEntity = Weapsy.Data.Entities.PageModule;
using PageModuleLocalisationDbEntity = Weapsy.Data.Entities.PageModuleLocalisation;
using PageModulePermissionDbEntity = Weapsy.Data.Entities.PageModulePermission;
using PagePermissionDbEntity = Weapsy.Data.Entities.PagePermission;
using SiteDbEntity = Weapsy.Data.Entities.Site;
using SiteLocalisationDbEntity = Weapsy.Data.Entities.SiteLocalisation;
using ThemeDbEntity = Weapsy.Data.Entities.Theme;
using UserDbEntity = Weapsy.Data.Entities.User;

namespace Weapsy.Data.Domain
{
    public class DomainAutoMapperProfile : Profile
    {
        public DomainAutoMapperProfile()
        {
            CreateMap<App, AppDbEntity>();
            CreateMap<AppDbEntity, App>().ConstructUsing(x => new App());

            CreateMap<Language, LanguageDbEntity>();
            CreateMap<LanguageDbEntity, Language>().ConstructUsing(x => new Language());

            CreateMap<Menu, MenuDbEntity>();
            CreateMap<MenuDbEntity, Menu>().ConstructUsing(x => new Menu())/*.ForMember(dest => dest.MenuItems, opt => opt.Ignore())*/;

            CreateMap<MenuItem, MenuItemDbEntity>();
            CreateMap<MenuItemDbEntity, MenuItem>().ConstructUsing(x => new MenuItem())/*.ForMember(dest => dest.MenuItemLocalisations, opt => opt.Ignore())*/;

            CreateMap<MenuItemLocalisation, MenuItemLocalisationDbEntity>();
            CreateMap<MenuItemLocalisationDbEntity, MenuItemLocalisation>().ConstructUsing(x => new MenuItemLocalisation());

            CreateMap<MenuItemPermission, MenuItemPermissionDbEntity>();
            CreateMap<MenuItemPermissionDbEntity, MenuItemPermission>().ConstructUsing(x => new MenuItemPermission());

            CreateMap<Module, ModuleDbEntity>();
            CreateMap<ModuleDbEntity, Module>().ConstructUsing(x => new Module());

            CreateMap<ModuleType, ModuleTypeDbEntity>();
            CreateMap<ModuleTypeDbEntity, ModuleType>().ConstructUsing(x => new ModuleType());

            CreateMap<Page, PageDbEntity>();
            CreateMap<PageDbEntity, Page>().ConstructUsing(x => new Page());

            CreateMap<PageLocalisation, PageLocalisationDbEntity>();
            CreateMap<PageLocalisationDbEntity, PageLocalisation>().ConstructUsing(x => new PageLocalisation());

            CreateMap<PageModule, PageModuleDbEntity>();
            CreateMap<PageModuleDbEntity, PageModule>().ConstructUsing(x => new PageModule());

            CreateMap<PageModuleLocalisation, PageModuleLocalisationDbEntity>();
            CreateMap<PageModuleLocalisationDbEntity, PageModuleLocalisation>().ConstructUsing(x => new PageModuleLocalisation());

            CreateMap<PageModulePermission, PageModulePermissionDbEntity>();
            CreateMap<PageModulePermissionDbEntity, PageModulePermission>().ConstructUsing(x => new PageModulePermission());

            CreateMap<PagePermission, PagePermissionDbEntity>();
            CreateMap<PagePermissionDbEntity, PagePermission>().ConstructUsing(x => new PagePermission());

            CreateMap<Site, SiteDbEntity>();
            CreateMap<SiteDbEntity, Site>().ConstructUsing(x => new Site());

            CreateMap<SiteLocalisation, SiteLocalisationDbEntity>();
            CreateMap<SiteLocalisationDbEntity, SiteLocalisation>().ConstructUsing(x => new SiteLocalisation());

            CreateMap<Theme, ThemeDbEntity>();
            CreateMap<ThemeDbEntity, Theme>().ConstructUsing(x => new Theme());

            CreateMap<User, UserDbEntity>();
            CreateMap<UserDbEntity, User>().ConstructUsing(x => new User());
        }
    }
}