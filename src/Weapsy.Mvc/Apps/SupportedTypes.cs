using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Weapsy.Mvc.Apps
{
    public static class SupportedType
    {
        private static readonly IList<Type> Types = new List<Type>();
    }
}
