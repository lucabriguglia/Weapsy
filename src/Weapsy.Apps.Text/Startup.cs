using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weapsy.Apps.Text.Extensions;
using Weapsy.Mvc.Apps;

namespace Weapsy.Apps.Text
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddEntityFramework(configuration);
        }

        public override void Configure(IApplicationBuilder builder)
        {
            builder.EnsureDbCreated();
            builder.EnsureAppInstalled();
        }
    }
}
