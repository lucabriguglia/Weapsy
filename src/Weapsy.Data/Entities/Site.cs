using System;
using System.Collections.Generic;
using Weapsy.Domain.Sites;

namespace Weapsy.Data.Entities
{
    public class Site
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Copyright { get; set; }
        public string Logo { get; set; }
        public string Title { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public Guid HomePageId { get; set; }
        public Guid ThemeId { get; set; }
        public Guid PageTemplateId { get; set; }
        public Guid ModuleTemplateId { get; set; }
        public bool AddLanguageSlug { get; set; }
        public SiteStatus Status { get; set; }

        public virtual ICollection<SiteLocalisation> SiteLocalisations { get; set; } = new List<SiteLocalisation>();
        public virtual ICollection<Page> Pages { get; set; } = new List<Page>();
        public virtual ICollection<Module> Modules { get; set; } = new List<Module>();
    }
}