using System;
using System.Collections.Generic;
using Weapsy.Domain.Languages;

namespace Weapsy.Reporting.Sites
{
    public class SiteAdminModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public Guid HomePageId { get; set; }
        public Guid ThemeId { get; set; }
        public Guid PageTemplateId { get; set; }
        public Guid ModuleTemplateId { get; set; }
        public bool AddLanguageSlug { get; set; }

        public List<SiteLocalisationAdminModel> SiteLocalisations { get; set; } = new List<SiteLocalisationAdminModel>();
        public List<PageListAdminModel> Pages { get; set; } = new List<PageListAdminModel>();
        public List<ThemeListAdminModel> Themes { get; set; } = new List<ThemeListAdminModel>();
    }

    public class SiteLocalisationAdminModel
    {
        public Guid SiteId { get; set; }
        public Guid LanguageId { get; set; }
        public string LanguageName { get; set; }
        public LanguageStatus LanguageStatus { get; set; }
        public string Title { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
    }

    public class PageListAdminModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class ThemeListAdminModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
