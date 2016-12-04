using System;
using System.Collections.Generic;
using System.Reflection;

namespace Weapsy.Mvc.Apps
{
    public class AppDescriptor
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Folder { get; set; }
        private IList<Assembly> Assemblies { get; set; } = new List<Assembly>();
    }
}
