#region Copyright

//  Weapsy - http://www.weapsy.org
//  Copyright (c) 2011-2013 Luca Briguglia
//  Licensed under the Weapsy Public License Version 1.0 (WPL-1.0)
//  A copy of this license may be found at http://www.weapsy.org/license

#endregion

using System.Collections.Generic;

namespace Weapsy.Entities
{
    public class PluginDefinition : BaseAuditEntity
    {
        public PluginDefinition()
        {
            Plugins = new HashSet<Plugin>();
        }
    
        public int AppId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public string DashboardText { get; set; }
        public string DashboardTitle { get; set; }
        public string MenuText { get; set; }
        public string MenuTitle { get; set; }

        public string Controller { get; set; }
        public string Action { get; set; }
        public string AdminController { get; set; }
        public string AdminAction { get; set; }

        public byte TypeId { get; set; }

        public string DashboardIcon { get; set; }
        public string MenuIcon { get; set; }

        public bool Deleted { get; set; }

        public virtual ICollection<Plugin> Plugins { get; set; }
    }

    public enum PluginType
    { 
        Account = 1
    }
}