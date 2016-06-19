#region Copyright

//  Weapsy - http://www.weapsy.org
//  Copyright (c) 2011-2013 Luca Briguglia
//  Licensed under the Weapsy Public License Version 1.0 (WPL-1.0)
//  A copy of this license may be found at http://www.weapsy.org/license

#endregion

namespace Weapsy.Entities
{
    using System.Collections.Generic;

    public class Text : BaseAuditEntity
    {
        public Text()
        {
            TextVersions = new HashSet<TextVersion>();
        }
    
        //public int SiteId { get; set; }
        public int ModuleId { get; set; }
        public int TextVersionId { get; set; }
    
        public virtual Module Module { get; set; }
        //public virtual TextVersion TextVersion { get; set; }
        public virtual ICollection<TextVersion> TextVersions { get; set; }
    }
}