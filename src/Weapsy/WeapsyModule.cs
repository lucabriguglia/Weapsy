using System.Reflection;
using Autofac;
using FluentValidation;
using Weapsy.Data;
using Weapsy.Data.Identity;
using Weapsy.Data.Providers;
using Weapsy.Data.Reporting.Apps;
using Weapsy.Data.Reporting.Languages;
using Weapsy.Data.Reporting.Menus;
using Weapsy.Data.Reporting.Modules;
using Weapsy.Data.Reporting.ModuleTypes;
using Weapsy.Data.Reporting.Pages;
using Weapsy.Data.Reporting.Sites;
using Weapsy.Data.Reporting.Themes;
using Weapsy.Data.Repositories;
using Weapsy.Domain.EventStore.SqlServer;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Sites.Handlers;
using Weapsy.Domain.Sites.Rules;
using Weapsy.Domain.Sites.Validators;
using Weapsy.Domain.Users.Handlers;
using Weapsy.Infrastructure.Caching;
using Weapsy.Infrastructure.DependencyResolver;
using Weapsy.Infrastructure.Dispatcher;
using Weapsy.Infrastructure.Domain;
using Weapsy.Reporting.Apps;
using Weapsy.Reporting.Languages;
using Weapsy.Reporting.Menus;
using Weapsy.Reporting.Modules;
using Weapsy.Reporting.ModuleTypes;
using Weapsy.Reporting.Pages;
using Weapsy.Reporting.Sites;
using Weapsy.Reporting.Themes;
using Weapsy.Services.Installation;

namespace Weapsy
{
    public class WeapsyModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DbContextFactory>().As<IDbContextFactory>();
            builder.RegisterType<EventStoreDbContext>().As<EventStoreDbContext>();

            builder.RegisterType<AutofacResolver>().As<IResolver>();
            builder.RegisterType<CommandSender>().As<ICommandSender>();
            builder.RegisterType<EventPublisher>().As<IEventPublisher>();
            builder.RegisterType<MemoryCacheManager>().As<ICacheManager>();
            builder.RegisterType<SqlServerEventStore>().As<IEventStore>();

            builder.RegisterAssemblyTypes(typeof(CreateSiteHandler).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(ICommandHandler<>));
            builder.RegisterAssemblyTypes(typeof(CreateSiteHandler).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(ICommandHandlerAsync<>));
            builder.RegisterAssemblyTypes(typeof(CreateSiteValidator).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IValidator<>));
            builder.RegisterAssemblyTypes(typeof(SiteRules).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRules<>));
            builder.RegisterAssemblyTypes(typeof(SiteRepository).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRepository<>));
            builder.RegisterAssemblyTypes(typeof(SiteEventsHandler).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IEventHandler<>));
            builder.RegisterAssemblyTypes(typeof(SiteEventsHandler).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IEventHandlerAsync<>));
            builder.RegisterAssemblyTypes(typeof(UserRegisteredHandler).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IEventHandler<>));
            builder.RegisterAssemblyTypes(typeof(UserRegisteredHandler).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IEventHandlerAsync<>));

            builder.RegisterType<MSSQLDataProvider>().As<IDataProvider>();

            builder.RegisterType<LanguageSortOrderGenerator>().As<ILanguageSortOrderGenerator>();

            builder.RegisterType<AppInstallationService>().As<IAppInstallationService>();
            builder.RegisterType<SiteInstallationService>().As<ISiteInstallationService>();
            builder.RegisterType<MembershipInstallationService>().As<IMembershipInstallationService>();

            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<RoleService>().As<IRoleService>();

            builder.RegisterType<AppFacade>().As<IAppFacade>();
            builder.RegisterType<LanguageFacade>().As<ILanguageFacade>();
            builder.RegisterType<MenuFacade>().As<IMenuFacade>();
            builder.RegisterType<ModuleFacade>().As<IModuleFacade>();
            builder.RegisterType<ModuleTypeFacade>().As<IModuleTypeFacade>();
            builder.RegisterType<PageFacade>().As<IPageFacade>();
            builder.RegisterType<SiteFacade>().As<ISiteFacade>();
            builder.RegisterType<ThemeFacade>().As<IThemeFacade>();

            builder.RegisterType<PageInfoFactory>().As<IPageInfoFactory>();
            builder.RegisterType<PageAdminFactory>().As<IPageAdminFactory>();
        }
    }
}
