#region Copyright

//  Weapsy - http://www.weapsy.org
//  Copyright (c) 2011-2013 Luca Briguglia
//  Licensed under the Weapsy Public License Version 1.0 (WPL-1.0)
//  A copy of this license may be found at http://www.weapsy.org/license

#endregion

namespace Weapsy.Entities
{
    public class LocalizedPage : BaseEntity
    {
        public int PageId { get; set; }
        public int LanguageId { get; set; }

        public string Url { get; set; }

        public string PageTitle { get; set; }
        public string PageImage { get; set; }

        public string MenuText { get; set; }
        public string MenuTitle { get; set; }
        public string MenuImage { get; set; }

        public string Title { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }

        public string Body { get; set; }
    
        public virtual Language Language { get; set; }
        public virtual Page Page { get; set; }
    }
}