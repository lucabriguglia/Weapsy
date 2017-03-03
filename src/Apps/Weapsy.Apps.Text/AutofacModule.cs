using System.Reflection;
using Autofac;
using FluentValidation;
using Weapsy.Apps.Text.Data;
using Weapsy.Infrastructure.Commands;
using Weapsy.Infrastructure.Events;
using Weapsy.Infrastructure.Domain;
using Weapsy.Infrastructure.Queries;

namespace Weapsy.Apps.Text
{
    public class AutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof(AutofacModule).GetTypeInfo().Assembly;

            builder.RegisterType<TextDbContext>().As<TextDbContext>();

            builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(IValidator<>));
            builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(IRules<>));
            builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(IRepository<>));
            builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(ICommandHandler<>));
            builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(ICommandHandlerAsync<>));
            builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(IEventHandler<>));
            builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(IEventHandlerAsync<>));
            builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(IQueryHandler<,>));
            builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(IQueryHandlerAsync<,>));

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces();
        }
    }
}
