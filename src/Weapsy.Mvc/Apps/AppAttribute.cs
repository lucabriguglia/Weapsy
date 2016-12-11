using System;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Weapsy.Mvc.Apps
{
    public class AppAttribute : RouteValueAttribute
    {
        public AppAttribute(string appName)
            : base("area", appName)
        {
            if (string.IsNullOrEmpty(appName))
            {
                throw new ArgumentException("App name must not be empty", nameof(appName));
            }
        }
    }
}
