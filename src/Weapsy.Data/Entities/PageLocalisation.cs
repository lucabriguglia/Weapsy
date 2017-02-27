using System;

namespace Weapsy.Data.Entities
{
    public class PageLocalisation
    {
        public Guid PageId { get; set; }
        public Guid LanguageId { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }

        public virtual Page Page { get; set; }
    }
}
