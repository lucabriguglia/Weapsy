#region Copyright

//  Weapsy - http://www.weapsy.org
//  Copyright (c) 2011-2013 Luca Briguglia
//  Licensed under the Weapsy Public License Version 1.0 (WPL-1.0)
//  A copy of this license may be found at http://www.weapsy.org/license

#endregion

namespace Weapsy.Entities
{    
    public class App : BaseAuditEntity
    {
        //public App()
        //{
        //    ModuleDefinitions = new HashSet<ModuleDefinition>();
        //    PluginDefinitions = new HashSet<PluginDefinition>();
        //    WidgetDefinitions = new HashSet<WidgetDefinition>();
        //}

        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Folder { get; set; }
        public string Url { get; set; }
        public string AdminController { get; set; }
        public string AdminAction { get; set; }
        public string DashboardIcon { get; set; }
        public bool Deleted { get; set; }

        //public virtual Nullable<ICollection<ModuleDefinition>> ModuleDefinitions { get; set; }
        //public virtual ICollection<PluginDefinition> PluginDefinitions { get; set; }
        //public virtual ICollection<WidgetDefinition> WidgetDefinitions { get; set; }
    }
}