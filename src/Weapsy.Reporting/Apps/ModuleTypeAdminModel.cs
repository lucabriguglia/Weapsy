using System;
using System.Collections.Generic;
using Weapsy.Domain.ModuleTypes;

namespace Weapsy.Reporting.Apps
{
    public class ModuleTypeAdminModel
    {
        public Guid Id { get; set; }
        public Guid AppId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ViewType ViewType { get; set; }
        public string ViewName { get; set; }
        public EditType EditType { get; set; }
        public string EditUrl { get; set; }
        public List<App> AvailableApps { get; set; } = new List<App>();

        public class App
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }
    }
}
