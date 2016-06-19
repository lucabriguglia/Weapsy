#region Copyright

//  Weapsy - http://www.weapsy.org
//  Copyright (c) 2011-2013 Luca Briguglia
//  Licensed under the Weapsy Public License Version 1.0 (WPL-1.0)
//  A copy of this license may be found at http://www.weapsy.org/license

#endregion

namespace Weapsy.Entities
{
    using System.Collections.Generic;

    public class MailTemplate : BaseAuditEntity
    {
        public MailTemplate()
        {
            LocalizedMailTemplates = new HashSet<LocalizedMailTemplate>();
        }

        public int MailTemplateTypeId { get; set; }
        public int SiteId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Subject { get; set; }
        public string BodyHtml { get; set; }
        public string BodyPlain { get; set; }
        public bool Deleted { get; set; }

        public MailTemplateType MailTemplateType { get; set; }
        public Site Site { get; set; }
        public virtual ICollection<LocalizedMailTemplate> LocalizedMailTemplates { get; set; }

        public string LocalizedSubject { get; set; }
        public string LocalizedBodyHtml { get; set; }
        public string LocalizedBodyPlain { get; set; }
    }
}