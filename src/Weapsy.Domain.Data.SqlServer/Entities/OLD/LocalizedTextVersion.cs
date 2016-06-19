#region Copyright

//  Weapsy - http://www.weapsy.org
//  Copyright (c) 2011-2013 Luca Briguglia
//  Licensed under the Weapsy Public License Version 1.0 (WPL-1.0)
//  A copy of this license may be found at http://www.weapsy.org/license

#endregion

namespace Weapsy.Entities
{
    public class LocalizedTextVersion : BaseEntity
    {
        public int TextVersionId { get; set; }
        public int LanguageId { get; set; }
        public string Body { get; set; }
    
        public virtual Language Language { get; set; }
        public virtual TextVersion TextVersion { get; set; }
    }
}