using Microsoft.Extensions.DependencyInjection;

namespace Weapsy.Web.Extensions
{
    public interface IWeapsyServiceBuilder
    {
        IServiceCollection Services { get; }
    }
}
