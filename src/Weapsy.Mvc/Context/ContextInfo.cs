using Weapsy.Reporting.Languages;
using Weapsy.Reporting.Sites;
using Weapsy.Reporting.Themes;
using Weapsy.Reporting.Users;

namespace Weapsy.Mvc.Context
{
    public class ContextInfo
    {
        public SiteInfo Site { get; set; }
        public UserInfo User { get; set; }
        public ThemeInfo Theme { get; set; }
        public LanguageInfo Language { get; set; }
    }
}
