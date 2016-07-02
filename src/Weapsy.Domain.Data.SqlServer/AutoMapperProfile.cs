using AutoMapper;
using Weapsy.Domain.Model.Apps;
using Weapsy.Domain.Model.Menus;
using Weapsy.Domain.Model.Languages;
using Weapsy.Domain.Model.Modules;
using Weapsy.Domain.Model.ModuleTypes;
using Weapsy.Domain.Model.Pages;
using Weapsy.Domain.Model.Sites;
using Weapsy.Domain.Model.Themes;
using AppDbEntity = Weapsy.Domain.Data.Entities.App;
using LanguageDbEntity = Weapsy.Domain.Data.Entities.Language;
using MenuDbEntity = Weapsy.Domain.Data.Entities.Menu;
using MenuItemDbEntity = Weapsy.Domain.Data.Entities.MenuItem;
using MenuItemLocalisationDbEntity = Weapsy.Domain.Data.Entities.MenuItemLocalisation;
using ModuleDbEntity = Weapsy.Domain.Data.Entities.Module;
using ModuleTypeDbEntity = Weapsy.Domain.Data.Entities.ModuleType;
using PageDbEntity = Weapsy.Domain.Data.Entities.Page;
using PageLocalisationDbEntity = Weapsy.Domain.Data.Entities.PageLocalisation;
using PageModuleDbEntity = Weapsy.Domain.Data.Entities.PageModule;
using PageModuleLocalisationDbEntity = Weapsy.Domain.Data.Entities.PageModuleLocalisation;
using SiteDbEntity = Weapsy.Domain.Data.Entities.Site;
using ThemeDbEntity = Weapsy.Domain.Data.Entities.Theme;
using UserDbEntity = Weapsy.Domain.Data.Entities.User;
using Weapsy.Domain.Model.Users;

namespace Weapsy.Domain.Data
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

            CreateMap<Site, SiteDbEntity>();
            CreateMap<SiteDbEntity, Site>().ConstructUsing(x => new Site());

            CreateMap<Theme, ThemeDbEntity>();
            CreateMap<ThemeDbEntity, Theme>().ConstructUsing(x => new Theme());

            CreateMap<User, UserDbEntity>();
            CreateMap<UserDbEntity, User>().ConstructUsing(x => new User());
        }
    }
}