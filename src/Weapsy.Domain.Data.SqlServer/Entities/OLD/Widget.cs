#region Copyright

//  Weapsy - http://www.weapsy.org
//  Copyright (c) 2011-2013 Luca Briguglia
//  Licensed under the Weapsy Public License Version 1.0 (WPL-1.0)
//  A copy of this license may be found at http://www.weapsy.org/license

#endregion

namespace Weapsy.Entities
{
    using System.Collections.Generic;

    public class Widget : BaseAuditEntity
    {
        public Widget()
        {
            LocalizedWidgets = new HashSet<LocalizedWidget>();
        }

        public int SiteId { get; set; }
        public int WidgetDefinitionId { get; set; }
        public string Title { get; set; }
        public int SortOrder { get; set; }
        public bool Active { get; set; }
        public string Authorized { get; set; }
        public bool Deleted { get; set; }

        public virtual Site Site { get; set; }
        public virtual WidgetDefinition WidgetDefinition { get; set; }
        public virtual ICollection<LocalizedWidget> LocalizedWidgets { get; set; }

        public string LocalizedTitle { get; set; }
    }
}