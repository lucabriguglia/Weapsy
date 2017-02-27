using System;

namespace Weapsy.Data.Entities
{
    public class SiteLocalisation
    {
        public Guid SiteId { get; set; }
        public Guid LanguageId { get; set; }
        public string Title { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }

        //public virtual Site Site { get; set; }
        //public virtual Language Language { get; set; }
    }
}
