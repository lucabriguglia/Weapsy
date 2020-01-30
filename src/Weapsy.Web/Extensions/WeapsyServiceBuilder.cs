using System;
using Microsoft.Extensions.DependencyInjection;

namespace Weapsy.Web.Extensions
{
    public class WeapsyServiceBuilder : IWeapsyServiceBuilder
    {
        public IServiceCollection Services { get; }

        public WeapsyServiceBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }
    }
}