using System;
using Weapsy.Domain.Languages;

namespace Weapsy.Data.Entities
{
    public class Language
    {
        public Guid SiteId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CultureName { get; set; }
        public string Url { get; set; }
        public int SortOrder { get; set; }
        public LanguageStatus Status { get; set; }

        //public virtual Site Site { get; set; }
    }
}