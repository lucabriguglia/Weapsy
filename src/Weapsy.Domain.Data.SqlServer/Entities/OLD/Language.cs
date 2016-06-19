#region Copyright

//  Weapsy - http://www.weapsy.org
//  Copyright (c) 2011-2013 Luca Briguglia
//  Licensed under the Weapsy Public License Version 1.0 (WPL-1.0)
//  A copy of this license may be found at http://www.weapsy.org/license

#endregion

namespace Weapsy.Entities
{
    using System.Collections.Generic;

    public class Language : BaseAuditEntity
    {
        public Language()
        {
            LocalizedMailTemplates = new HashSet<LocalizedMailTemplate>();
            LocalizedModules = new HashSet<LocalizedModule>();
            LocalizedPages = new HashSet<LocalizedPage>();
            LocalizedPageModules = new HashSet<LocalizedPageModule>();
            LocalizedPlugins = new HashSet<LocalizedPlugin>();
            LocalizedSites = new HashSet<LocalizedSite>();
            LocalizedTextVersions = new HashSet<LocalizedTextVersion>();
            LocalizedWidgets = new HashSet<LocalizedWidget>();
        }
    
        public int SiteId { get; set; }
        public string Name { get; set; }
        public string CultureName { get; set; }
        public string CultureNativeName { get; set; }
        public string CultureEnglishName { get; set; }
        public string Url { get; set; }
        public string Flag { get; set; }
        public bool Active { get; set; }
        public int SortOrder { get; set; }
        public bool Deleted { get; set; }

        public virtual Site Site { get; set; }

        public virtual ICollection<LocalizedMailTemplate> LocalizedMailTemplates { get; set; }
        public virtual ICollection<LocalizedModule> LocalizedModules { get; set; }
        public virtual ICollection<LocalizedPage> LocalizedPages { get; set; }
        public virtual ICollection<LocalizedPageModule> LocalizedPageModules { get; set; }
        public virtual ICollection<LocalizedPlugin> LocalizedPlugins { get; set; }
        public virtual ICollection<LocalizedSite> LocalizedSites { get; set; }
        public virtual ICollection<LocalizedTextVersion> LocalizedTextVersions { get; set; }
        public virtual ICollection<LocalizedWidget> LocalizedWidgets { get; set; }
    }
}