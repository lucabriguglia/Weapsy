using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Weapsy.Reporting.Sites;
using Weapsy.Reporting.Themes;
using Microsoft.AspNetCore.Localization;
using Weapsy.Reporting.Languages;

namespace Weapsy.Mvc.Context
{
    public class ContextService : IContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISiteFacade _siteFacade;
        private readonly IThemeFacade _themeFacade;

        public ContextService(IHttpContextAccessor httpContextAccessor, 
            ISiteFacade siteFacade,
            IThemeFacade themeFacade)
        {
            _httpContextAccessor = httpContextAccessor;
            _siteFacade = siteFacade;
            _themeFacade = themeFacade;
        }

        private const string ContextInfoKey = "Weapsy|ContextInfo";

        public ContextInfo GetCurrentContextInfo()
        {
            if (_httpContextAccessor.HttpContext.Items[ContextInfoKey] == null)
            {
                _httpContextAccessor.HttpContext.Items.Add(ContextInfoKey, GetContextInfo());
            }
            return (ContextInfo)_httpContextAccessor.HttpContext.Items[ContextInfoKey];
        }

        private ContextInfo GetContextInfo()
        {
            return new ContextInfo
            {
                Site = GetSiteInfo(),
                Language = GetLanguageInfo()
            };
        }

        private SiteInfo GetSiteInfo()
        {
            return _siteFacade.GetSiteInfo("Default").Result;
        }

        private LanguageInfo GetLanguageInfo()
        {
            var requestedLanguageId = _httpContextAccessor.HttpContext.GetRouteValue(ContextKeys.LanguageKey);

            var userCulture = _httpContextAccessor.HttpContext.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];

            return new LanguageInfo();
        }
    }
}
