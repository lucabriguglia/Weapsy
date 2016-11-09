using Weapsy.Reporting.Languages;
using Weapsy.Reporting.Sites;
using Weapsy.Reporting.Themes;
using Weapsy.Reporting.Users;

namespace Weapsy.Mvc.Context
{
    public interface IContextService
    {
        ContextInfo GetCurrentContextInfo();
        SiteInfo GetCurrentSiteInfo();
        LanguageInfo GetCurrentLanguageInfo();
        ThemeInfo GetCurrentThemeInfo();
        UserInfo GetCurrentUserInfo();
    }
}
