using System;
using System.Collections.Generic;
using Weapsy.Domain.Apps;
using Weapsy.Domain.ModuleTypes;

namespace Weapsy.Reporting.Apps
{
    public class AppAdminModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Folder { get; set; }
        public AppStatus Status { get; set; }
        public IList<ModuleType> ModuleTypes { get; set; } = new List<ModuleType>();
    }
}
