#region Copyright

//  Weapsy - http://www.weapsy.org
//  Copyright (c) 2011-2013 Luca Briguglia
//  Licensed under the Weapsy Public License Version 1.0 (WPL-1.0)
//  A copy of this license may be found at http://www.weapsy.org/license

#endregion

namespace Weapsy.Entities
{
    public class PageTemplate : BaseAuditEntity
    {
        //public PageTemplate()
        //{
        //    Sites = new HashSet<Site>();
        //    Pages = new HashSet<Page>();
        //}

        public string Name { get; set; }
        public string Zones { get; set; }
        public string ViewName { get; set; }
        public string PreviewImage { get; set; }
        public bool Deleted { get; set; }

        //public virtual ICollection<Site> Sites { get; set; }
        //public virtual ICollection<Page> Pages { get; set; }
    }
}