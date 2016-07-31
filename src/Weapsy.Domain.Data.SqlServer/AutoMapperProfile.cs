using AutoMapper;
using Weapsy.Domain.Model.Apps;
using Weapsy.Domain.Model.Languages;
using Weapsy.Domain.Model.Menus;
using Weapsy.Domain.Model.Modules;
using Weapsy.Domain.Model.ModuleTypes;
using Weapsy.Domain.Model.Pages;
using Weapsy.Domain.Model.Sites;
using Weapsy.Domain.Model.Themes;
using Weapsy.Domain.Model.Users;
using AppDbEntity = Weapsy.Domain.Data.SqlServer.Entities.App;
using LanguageDbEntity = Weapsy.Domain.Data.SqlServer.Entities.Language;
using MenuDbEntity = Weapsy.Domain.Data.SqlServer.Entities.Menu;
using MenuItemDbEntity = Weapsy.Domain.Data.SqlServer.Entities.MenuItem;
using MenuItemLocalisationDbEntity = Weapsy.Domain.Data.SqlServer.Entities.MenuItemLocalisation;
using ModuleDbEntity = Weapsy.Domain.Data.SqlServer.Entities.Module;
using ModuleTypeDbEntity = Weapsy.Domain.Data.SqlServer.Entities.ModuleType;
using PageDbEntity = Weapsy.Domain.Data.SqlServer.Entities.Page;
using PageLocalisationDbEntity = Weapsy.Domain.Data.SqlServer.Entities.PageLocalisation;
using PageModuleDbEntity = Weapsy.Domain.Data.SqlServer.Entities.PageModule;
using PageModuleLocalisationDbEntity = Weapsy.Domain.Data.SqlServer.Entities.PageModuleLocalisation;
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