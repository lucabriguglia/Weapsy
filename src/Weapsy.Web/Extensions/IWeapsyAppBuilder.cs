using Microsoft.AspNetCore.Builder;

namespace Weapsy.Web.Extensions
{
    public interface IWeapsyAppBuilder
    {
        IApplicationBuilder App { get; }
    }
}