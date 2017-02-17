using System.Reflection;
using Autofac;
using FluentValidation;
using Weapsy.Data;
using Weapsy.Domain.Sites.Commands;
using Weapsy.Infrastructure.Commands;
using Weapsy.Infrastructure.Domain;
using Weapsy.Infrastructure.Events;
using Weapsy.Infrastructure.Queries;
using Weapsy.Reporting.Apps.Queries;
using Weapsy.Services.Identity;
using Weapsy.Services.Installation;

namespace Weapsy
{
    public class AutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var infrastructureAssembly = typeof(AggregateRoot).GetTypeInfo().Assembly;
            var domainAssembly = typeof(CreateSite).GetTypeInfo().Assembly;
            var dataAssembly = typeof(IDataProvider).GetTypeInfo().Assembly;
            var reportingAssembly = typeof(GetAppAdminModel).GetTypeInfo().Assembly;

            builder.RegisterAssemblyTypes(domainAssembly).AsClosedTypesOf(typeof(ICommandHandler<>));
            builder.RegisterAssemblyTypes(domainAssembly).AsClosedTypesOf(typeof(ICommandHandlerAsync<>));
            builder.RegisterAssemblyTypes(domainAssembly).AsClosedTypesOf(typeof(IValidator<>));
            builder.RegisterAssemblyTypes(domainAssembly).AsClosedTypesOf(typeof(IRules<>));
            builder.RegisterAssemblyTypes(domainAssembly).AsClosedTypesOf(typeof(IEventHandler<>));
            builder.RegisterAssemblyTypes(domainAssembly).AsClosedTypesOf(typeof(IEventHandlerAsync<>));

            builder.RegisterAssemblyTypes(dataAssembly).AsClosedTypesOf(typeof(IRepository<>));
            builder.RegisterAssemblyTypes(dataAssembly).AsClosedTypesOf(typeof(IEventHandler<>));
            builder.RegisterAssemblyTypes(dataAssembly).AsClosedTypesOf(typeof(IEventHandlerAsync<>));
            builder.RegisterAssemblyTypes(dataAssembly).AsClosedTypesOf(typeof(IQueryHandler<,>));
            builder.RegisterAssemblyTypes(dataAssembly).AsClosedTypesOf(typeof(IQueryHandlerAsync<,>));

            builder.RegisterAssemblyTypes(reportingAssembly).AsClosedTypesOf(typeof(IEventHandler<>));
            builder.RegisterAssemblyTypes(reportingAssembly).AsClosedTypesOf(typeof(IEventHandlerAsync<>));
            //builder.RegisterAssemblyTypes(reportingAssembly).AsClosedTypesOf(typeof(IQueryHandler<,>));
            //builder.RegisterAssemblyTypes(reportingAssembly).AsClosedTypesOf(typeof(IQueryHandlerAsync<,>));

            builder.RegisterAssemblyTypes(infrastructureAssembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(domainAssembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(dataAssembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(reportingAssembly).AsImplementedInterfaces();





            //builder.RegisterType<DbContextFactory>().As<IDbContextFactory>();

            //builder.RegisterType<AutofacResolver>().As<IResolver>();
            //builder.RegisterType<CommandSender>().As<ICommandSender>();
            //builder.RegisterType<EventPublisher>().As<IEventPublisher>();
            //builder.RegisterType<MemoryCacheManager>().As<ICacheManager>();
            //builder.RegisterType<EventStore>().As<IEventStore>();

            //builder.RegisterAssemblyTypes(typeof(CreateSiteHandler).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(ICommandHandler<>));
            //builder.RegisterAssemblyTypes(typeof(CreateSiteHandler).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(ICommandHandlerAsync<>));
            //builder.RegisterAssemblyTypes(typeof(CreateSiteValidator).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IValidator<>));
            //builder.RegisterAssemblyTypes(typeof(SiteRules).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRules<>));
            //builder.RegisterAssemblyTypes(typeof(SiteRepository).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRepository<>));
            //builder.RegisterAssemblyTypes(typeof(SiteEventsHandler).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IEventHandler<>));
            //builder.RegisterAssemblyTypes(typeof(SiteEventsHandler).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IEventHandlerAsync<>));
            //builder.RegisterAssemblyTypes(typeof(UserRegisteredHandler).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IEventHandler<>));
            //builder.RegisterAssemblyTypes(typeof(UserRegisteredHandler).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IEventHandlerAsync<>));

            //builder.RegisterType<LanguageSortOrderGenerator>().As<ILanguageSortOrderGenerator>();

            builder.RegisterType<AppInstallationService>().As<IAppInstallationService>();
            builder.RegisterType<SiteInstallationService>().As<ISiteInstallationService>();
            builder.RegisterType<MembershipInstallationService>().As<IMembershipInstallationService>();

            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<RoleService>().As<IRoleService>();

            //builder.RegisterType<LanguageFacade>().As<ILanguageFacade>();
            //builder.RegisterType<MenuFacade>().As<IMenuFacade>();
            //builder.RegisterType<ModuleFacade>().As<IModuleFacade>();
            //builder.RegisterType<ModuleTypeFacade>().As<IModuleTypeFacade>();
            //builder.RegisterType<PageFacade>().As<IPageFacade>();
            //builder.RegisterType<SiteFacade>().As<ISiteFacade>();
            //builder.RegisterType<ThemeFacade>().As<IThemeFacade>();

            //builder.RegisterType<PageInfoFactory>().As<IPageInfoFactory>();
            //builder.RegisterType<PageAdminFactory>().As<IPageAdminFactory>();












        }
    }
}
