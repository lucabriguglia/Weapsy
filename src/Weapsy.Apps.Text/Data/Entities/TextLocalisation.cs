using System;

namespace Weapsy.Apps.Text.Data.Entities
{
    public class TextLocalisation
    {
        public Guid TextVersionId { get; set; }
        public Guid LanguageId { get; set; }
        public string Content { get; set; }

        public virtual TextVersion TextVersion { get; set; }
    }
}
