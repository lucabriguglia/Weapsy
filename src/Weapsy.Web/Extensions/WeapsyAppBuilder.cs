using System;
using Microsoft.AspNetCore.Builder;

namespace Weapsy.Web.Extensions
{
    public class WeapsyAppBuilder : IWeapsyAppBuilder
    {
        public IApplicationBuilder App { get; }

        public WeapsyAppBuilder(IApplicationBuilder app)
        {
            App = app ?? throw new ArgumentNullException(nameof(app));
        }
    }
}