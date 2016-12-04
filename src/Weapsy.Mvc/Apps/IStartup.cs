using Microsoft.Extensions.DependencyInjection;

namespace Weapsy.Mvc.Apps
{
    public interface IStartup
    {
        void ConfigureServices(IServiceCollection serviceCollection);
    }
}
