#region Copyright

//  Weapsy - http://www.weapsy.org
//  Copyright (c) 2011-2013 Luca Briguglia
//  Licensed under the Weapsy Public License Version 1.0 (WPL-1.0)
//  A copy of this license may be found at http://www.weapsy.org/license

#endregion

using System.Collections.Generic;

namespace Weapsy.Entities
{
    public class Plugin : BaseAuditEntity
    {
        public Plugin()
        {
            LocalizedPlugins = new HashSet<LocalizedPlugin>();
        }

        public int SiteId { get; set; }

        public int PluginDefinitionId { get; set; }

        public string DashboardText { get; set; }
        public string DashboardTitle { get; set; }
        public string MenuText { get; set; }
        public string MenuTitle { get; set; }

        public int SortOrder { get; set; }
        public bool Active { get; set; }
        public string Authorized { get; set; }
        public bool Deleted { get; set; }

        public virtual Site Site { get; set; }
        public virtual PluginDefinition PluginDefinition { get; set; }
        public virtual ICollection<LocalizedPlugin> LocalizedPlugins { get; set; }

        public string LocalizedDashboardText { get; set; }
        public string LocalizedDashboardTitle { get; set; }
        public string LocalizedMenuText { get; set; }
        public string LocalizedMenuTitle { get; set; }
    }
}