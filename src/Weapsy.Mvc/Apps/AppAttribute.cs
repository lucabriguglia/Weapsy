using System;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Weapsy.Mvc.Apps
{
    public class AppAttribute : RouteValueAttribute
    {
        /// <summary>
        /// Initializes a new <see cref="AppAttribute"/> instance.
        /// </summary>
        /// <param name="appName">The app containing the controller or action.</param>
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
