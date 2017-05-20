using System;
using System.Collections.Generic;
using Weapsy.Framework.Commands;

namespace Weapsy.Domain.Sites.Commands
{
    public class UpdateSiteDetails : ICommand
    {
        public Guid SiteId { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public Guid HomePageId { get; set; }
        public Guid ThemeId { get; set; }
        public Guid PageTemplateId { get; set; }
        public Guid ModuleTemplateId { get; set; }
        public bool AddLanguageSlug { get; set; }

        public List<SiteLocalisation> SiteLocalisations { get; set; } = new List<SiteLocalisation>();
    }
}
