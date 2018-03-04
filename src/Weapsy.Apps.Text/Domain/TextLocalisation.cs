using System;

namespace Weapsy.Apps.Text.Domain
{
    public class TextLocalisation
    {
        public Guid TextVersionId { get; set; }
        public Guid LanguageId { get; set; }
        public string Content { get; set; }
    }
}
