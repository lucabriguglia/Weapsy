using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Weapsy.Mvc.Apps
{
    public interface IStartup
    {
        void ConfigureServices(IServiceCollection serviceCollection);
        void Configure(IApplicationBuilder builder);
    }
}
