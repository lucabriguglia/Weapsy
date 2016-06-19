using System;

namespace Weapsy.Domain.Data.Entities
{
    public class SiteLocalisation : IDbEntity
    {
        public Guid SiteId { get; set; }
        public Guid LanguageId { get; set; }
        public string Title { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
    }
}
