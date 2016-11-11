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
using AppDbEntity = Weapsy.Domain.Data.SqlServer.Entities.App;
using LanguageDbEntity = Weapsy.Domain.Data.SqlServer.Entities.Language;
using MenuDbEntity = Weapsy.Domain.Data.SqlServer.Entities.Menu;
using MenuItemDbEntity = Weapsy.Domain.Data.SqlServer.Entities.MenuItem;
using MenuItemLocalisationDbEntity = Weapsy.Domain.Data.SqlServer.Entities.MenuItemLocalisation;
using MenuItemPermissionDbEntity = Weapsy.Domain.Data.SqlServer.Entities.MenuItemPermission;
using ModuleDbEntity = Weapsy.Domain.Data.SqlServer.Entities.Module;
using ModuleTypeDbEntity = Weapsy.Domain.Data.SqlServer.Entities.ModuleType;
using PageDbEntity = Weapsy.Domain.Data.SqlServer.Entities.Page;
using PageLocalisationDbEntity = Weapsy.Domain.Data.SqlServer.Entities.PageLocalisation;
using PageModuleDbEntity = Weapsy.Domain.Data.SqlServer.Entities.PageModule;
using PageModuleLocalisationDbEntity = Weapsy.Domain.Data.SqlServer.Entities.PageModuleLocalisation;
using PageModulePermissionDbEntity = Weapsy.Domain.Data.SqlServer.Entities.PageModulePermission;
using PagePermissionDbEntity = Weapsy.Domain.Data.SqlServer.Entities.PagePermission;
using SiteDbEntity = Weapsy.Domain.Data.SqlServer.Entities.Site;
using ThemeDbEntity = Weapsy.Domain.Data.SqlServer.Entities.Theme;
using UserDbEntity = Weapsy.Domain.Data.SqlServer.Entities.User;

namespace Weapsy.Domain.Data.SqlServer
{
    public class AutoMapperProfile : Profile
    {
        protected override void Configure()
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

            CreateMap<Theme, ThemeDbEntity>();
            CreateMap<ThemeDbEntity, Theme>().ConstructUsing(x => new Theme());

            CreateMap<User, UserDbEntity>();
            CreateMap<UserDbEntity, User>().ConstructUsing(x => new User());
        }
    }
}