using System.Reflection;
using Autofac;
using FluentValidation;
using Weapsy.Data;
using Weapsy.Domain.Sites.Commands;
using Weapsy.Framework.Domain;
using Weapsy.Reporting.Apps.Queries;
using Weapsy.Services.Mail;

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
            var servicesAssembly = typeof(IMailService).GetTypeInfo().Assembly;

            //builder.RegisterAssemblyTypes(domainAssembly).AsClosedTypesOf(typeof(ICommandHandler<>));
            //builder.RegisterAssemblyTypes(domainAssembly).AsClosedTypesOf(typeof(ICommandHandlerAsync<>));
            //builder.RegisterAssemblyTypes(domainAssembly).AsClosedTypesOf(typeof(IValidator<>));
            //builder.RegisterAssemblyTypes(domainAssembly).AsClosedTypesOf(typeof(IRules<>));
            //builder.RegisterAssemblyTypes(domainAssembly).AsClosedTypesOf(typeof(IEventHandler<>));
            //builder.RegisterAssemblyTypes(domainAssembly).AsClosedTypesOf(typeof(IEventHandlerAsync<>));

            //builder.RegisterAssemblyTypes(dataAssembly).AsClosedTypesOf(typeof(IRepository<>));
            //builder.RegisterAssemblyTypes(dataAssembly).AsClosedTypesOf(typeof(IEventHandler<>));
            //builder.RegisterAssemblyTypes(dataAssembly).AsClosedTypesOf(typeof(IEventHandlerAsync<>));
            //builder.RegisterAssemblyTypes(dataAssembly).AsClosedTypesOf(typeof(IQueryHandler<,>));
            //builder.RegisterAssemblyTypes(dataAssembly).AsClosedTypesOf(typeof(IQueryHandlerAsync<,>));

            //builder.RegisterAssemblyTypes(reportingAssembly).AsClosedTypesOf(typeof(IEventHandler<>));
            //builder.RegisterAssemblyTypes(reportingAssembly).AsClosedTypesOf(typeof(IEventHandlerAsync<>));

            builder.RegisterAssemblyTypes(infrastructureAssembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(domainAssembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(dataAssembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(reportingAssembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(servicesAssembly).AsImplementedInterfaces();
        }
    }
}
