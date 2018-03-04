using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Weapsy.Mvc.Apps
{
    public interface IStartup
    {
        void ConfigureServices(IServiceCollection serviceCollection, IConfiguration configuration);
        void Configure(IApplicationBuilder builder);
    }
}
