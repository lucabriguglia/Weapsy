using System;
using System.Collections.Generic;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Sites.Events
{
    public class SiteDetailsUpdated : DomainEvent
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public bool AddLanguageSlug { get; set; }
        public Guid HomePageId { get; set; }
        public Guid ThemeId { get; set; }
        public IEnumerable<SiteLocalisation> SiteLocalisations { get; set; } = new List<SiteLocalisation>();
    }
}
