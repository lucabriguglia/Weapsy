#region Copyright

//  Weapsy - http://www.weapsy.org
//  Copyright (c) 2011-2013 Luca Briguglia
//  Licensed under the Weapsy Public License Version 1.0 (WPL-1.0)
//  A copy of this license may be found at http://www.weapsy.org/license

#endregion

namespace Weapsy.Entities
{
    public class LocalizedPlugin : BaseEntity
    {
        public int PluginId { get; set; }
        public int LanguageId { get; set; }
        public string DashboardText { get; set; }
        public string DashboardTitle { get; set; }
        public string MenuText { get; set; }
        public string MenuTitle { get; set; }
    
        public virtual Language Language { get; set; }
        public virtual Plugin Plugin { get; set; }
    }
}