using Autofac;
using FluentValidation;
using Weapsy.Domain.Sites.Handlers;
using Weapsy.Domain.Sites.Rules;
using Weapsy.Domain.Sites.Validators;
using Weapsy.Domain.Languages;
using Weapsy.Reporting.Menus;
using Weapsy.Reporting.Languages;
using Weapsy.Reporting.Pages;
using Weapsy.Reporting.Sites;
using Weapsy.Domain.EventStore.SqlServer;
using System.Reflection;
using Weapsy.Data;
using Weapsy.Data.Providers.MSSQL;
using Weapsy.Domain.Data.Repositories;
using Weapsy.Reporting.ModuleTypes;
using Weapsy.Reporting.Apps;
using Weapsy.Reporting.Modules;
using Weapsy.Reporting.Themes;
using Weapsy.Domain.Users.Handlers;
using Weapsy.Services.Identity;
using Weapsy.Services.Installation;
using Weapsy.Infrastructure.DependencyResolver;
using Weapsy.Infrastructure.Dispatcher;
using Weapsy.Infrastructure.Caching;
using Weapsy.Infrastructure.Domain;
using Weapsy.Reporting.Data.Apps;
using Weapsy.Reporting.Data.Languages;
using Weapsy.Reporting.Data.Menus;
using Weapsy.Reporting.Data.Modules;
using Weapsy.Reporting.Data.ModuleTypes;
using Weapsy.Reporting.Data.Pages;
using Weapsy.Reporting.Data.Sites;
using Weapsy.Reporting.Data.Themes;

namespace Weapsy.DependencyConfigurator
{
    public class WeapsyModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MSSQLDbContext>().As<MSSQLDbContext>();
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
