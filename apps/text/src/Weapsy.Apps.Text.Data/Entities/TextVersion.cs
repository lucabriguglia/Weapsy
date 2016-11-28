using System;
using System.Collections.Generic;
using Weapsy.Apps.Text.Domain;

namespace Weapsy.Apps.Text.Data.Entities
{
    public class TextVersion
    {
        public Guid Id { get; set; }
        public Guid TextModuleId { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public TextVersionStatus Status { get; set; }

        public IList<TextLocalisation> TextLocalisations { get; set; } = new List<TextLocalisation>();

        public virtual TextModule TextModule { get; set; }
    }
}
