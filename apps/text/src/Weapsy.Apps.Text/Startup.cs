using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Weapsy.Apps.Text
{
    public class Startup : StartupBase<IContainer>
    {
        public Startup(IServiceProviderFactory<IContainer> factory) : base(factory)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
        }

        public override void Configure(IApplicationBuilder app)
        {
        }
    }
}
