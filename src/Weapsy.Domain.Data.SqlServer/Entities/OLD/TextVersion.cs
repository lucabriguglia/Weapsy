#region Copyright

//  Weapsy - http://www.weapsy.org
//  Copyright (c) 2011-2013 Luca Briguglia
//  Licensed under the Weapsy Public License Version 1.0 (WPL-1.0)
//  A copy of this license may be found at http://www.weapsy.org/license

#endregion

namespace Weapsy.Entities
{
    using System.Collections.Generic;

    public class TextVersion : BaseAuditEntity
    {
        public TextVersion()
        {
            LocalizedTextVersions = new HashSet<LocalizedTextVersion>();
        }

        public int TextId { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }

        public virtual Text Text { get; set; }

        public virtual ICollection<LocalizedTextVersion> LocalizedTextVersions { get; set; }
    }
}