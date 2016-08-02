using System;

namespace Weapsy.Mvc.Context
{
    public class SiteInfo
    {
        public Guid SiteId { get; set; }
        public string SiteName { get; set; }
        public Guid UserId { get; set; }
        public Guid ThemeId { get; set; }
        public string ThemeName { get; set; }
        public string ThemeFolder { get; set; }
        public Guid LanguageId { get; set; }
        public string LanguageCulture { get; set; }
    }
}
