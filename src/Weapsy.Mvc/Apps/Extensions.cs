using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Weapsy.Mvc.Apps
{
    public static class Extensions
    {
        public static IList<T> GetTypes<T>(this IList<Assembly> assemblies)
        {
            var result = new List<T>();

            foreach (var assembly in assemblies)
            {
                var type = assembly.GetTypes().FirstOrDefault(t => typeof(T).IsAssignableFrom(t));

                if (type == null)
                    continue;

                var instance = (T)Activator.CreateInstance(type);

                result.Add(instance);
            }

            return result;
        }
    }
}
