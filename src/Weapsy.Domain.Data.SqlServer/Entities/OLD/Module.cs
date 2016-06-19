#region Copyright

//  Weapsy - http://www.weapsy.org
//  Copyright (c) 2011-2013 Luca Briguglia
//  Licensed under the Weapsy Public License Version 1.0 (WPL-1.0)
//  A copy of this license may be found at http://www.weapsy.org/license

#endregion

namespace Weapsy.Entities
{
    using System;
    using System.Collections.Generic;

    public class Module : BaseAuditEntity
    {
        public Module()
        {
            LocalizedModules = new HashSet<LocalizedModule>();
            PageModules = new HashSet<PageModule>();
            Texts = new HashSet<Text>();
        }
    
        public int SiteId { get; set; }
        public int ModuleDefinitionId { get; set; }
        public string Title { get; set; }
        public bool AllPages { get; set; }
        public bool Deleted { get; set; }
        public bool InheritPermissions { get; set; }
        public string Authorized { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool DisplayTitle { get; set; }
        public string Zone { get; set; }

        public Site Site { get; set; }
        public virtual ModuleDefinition ModuleDefinition { get; set; }
        public virtual ICollection<LocalizedModule> LocalizedModules { get; set; }
        public virtual ICollection<PageModule> PageModules { get; set; }
        public virtual ICollection<Text> Texts { get; set; }

        public string LocalizedTitle { get; set; }
    }
}