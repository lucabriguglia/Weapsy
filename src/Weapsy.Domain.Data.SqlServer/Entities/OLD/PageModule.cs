#region Copyright

//  Weapsy - http://www.weapsy.org
//  Copyright (c) 2011-2013 Luca Briguglia
//  Licensed under the Weapsy Public License Version 1.0 (WPL-1.0)
//  A copy of this license may be found at http://www.weapsy.org/license

#endregion

namespace Weapsy.Entities
{
    using System.Collections.Generic;

    public class PageModule : BaseAuditEntity
    {
        public PageModule()
        {
            LocalizedPageModules = new HashSet<LocalizedPageModule>();
        }

        public int PageId { get; set; }
        public int ModuleId { get; set; }
        public string Title { get; set; }
        public string Zone { get; set; }
        public int ModuleTemplateId { get; set; }
        public int SortOrder { get; set; }
        public bool Deleted { get; set; }
        public string Icon { get; set; }

        public virtual Module Module { get; set; }
        public virtual Page Page { get; set; }

        public virtual ICollection<LocalizedPageModule> LocalizedPageModules { get; set; }

        //public virtual ModuleTemplate ModuleTemplate { get; set; }

        public string LocalizedTitle { get; set; }
    }
}