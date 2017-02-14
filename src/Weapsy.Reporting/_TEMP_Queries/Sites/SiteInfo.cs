using System;

namespace Weapsy.Reporting.Sites
{
    public class SiteInfo
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
    }
}
