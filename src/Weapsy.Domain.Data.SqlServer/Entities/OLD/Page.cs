#region Copyright

//  Weapsy - http://www.weapsy.org
//  Copyright (c) 2011-2013 Luca Briguglia
//  Licensed under the Weapsy Public License Version 1.0 (WPL-1.0)
//  A copy of this license may be found at http://www.weapsy.org/license

#endregion

using System;
using System.Collections.Generic;

namespace Weapsy.Entities
{
    public class Page : BaseAuditEntity
    {
        public Page()
        {
            LocalizedPages = new HashSet<LocalizedPage>();
            PageModules = new HashSet<PageModule>();
        }
    
        public int SiteId { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public string PageTitle { get; set; }
        public string PageIcon { get; set; }
        public string PageImage { get; set; }

        public string MenuText { get; set; }
        public string MenuTitle { get; set; }
        public string MenuIcon { get; set; }
        public string MenuImage { get; set; }

        public string Title { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }

        public int PageTemplateId { get; set; }
        public int ModuleTemplateId { get; set; }

        public string Authorized { get; set; }

        public bool MainMenu { get; set; }
        public bool SiteMap { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public bool Disabled { get; set; }

        public int SortOrder { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool DisplayPageTitle { get; set; }
        public bool LanguageUrlEnabled { get; set; }
        
        public string Link { get; set; }
        
        //public string Body { get; set; }
        //public bool UseModules { get; set; }

        public Site Site { get; set; }
        //public ModuleTemplate ModuleTemplate { get; set; }
        //public PageTemplate PageTemplate { get; set; }

        public virtual ICollection<LocalizedPage> LocalizedPages { get; set; }
        public virtual ICollection<PageModule> PageModules { get; set; }

        public int PageModuleCount { get; set; }

        public string LocalizedUrl { get; set; }
        public string LocalizedPageTitle { get; set; }
        public string LocalizedPageImage { get; set; }
        public string LocalizedMenuText { get; set; }
        public string LocalizedMenuTitle { get; set; }
        public string LocalizedMenuImage { get; set; }
        public string LocalizedTitle { get; set; }
        public string LocalizedMetaDescription { get; set; }
        public string LocalizedMetaKeywords { get; set; }

        public string NameForTree { get; set; }
    }
}