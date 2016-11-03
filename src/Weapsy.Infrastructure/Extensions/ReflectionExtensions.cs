using System;
using System.Reflection;

namespace Weapsy.Infrastructure.Extensions
{
    public static class ReflectionExtensions
    {
        public static bool HasProperty(this object obj, string name)
        {
            return obj.GetType().GetRuntimeProperty(name) != null;
        }

        public static bool HasValue(this object obj, string name)
        {
            var currentProperty = obj.GetType().GetRuntimeProperty(name);
            if (currentProperty == null)
                return false;
            var caurrentValue = currentProperty.GetValue(obj);
            var defaultValue = Activator.CreateInstance(obj.GetType()).GetType().GetRuntimeProperty(name).GetValue(obj);            
            return caurrentValue != defaultValue;
        }

        public static void SetValue(this object obj, string name, object value)
        {
            obj.GetType().GetRuntimeProperty(name).SetValue(obj, value);
        }
    }
}
