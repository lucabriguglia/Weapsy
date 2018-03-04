using System.Reflection;
using Autofac;

namespace Weapsy.Apps.Text
{
    public class AutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof(AutofacModule).GetTypeInfo().Assembly;

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces();
        }
    }
}
