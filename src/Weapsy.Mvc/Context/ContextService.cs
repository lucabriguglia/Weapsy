using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using Weapsy.Reporting.Sites;
using Weapsy.Reporting.Themes;
using Microsoft.AspNetCore.Localization;

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

        private const string ContextKey = "Weapsy|SiteInfo";

        public SiteInfo GetCurrentSiteInfo()
        {
            if (_httpContextAccessor.HttpContext.Items[ContextKey] == null)
            {
                _httpContextAccessor.HttpContext.Items.Add(ContextKey, GetSiteInfo());
            }
            return (SiteInfo)_httpContextAccessor.HttpContext.Items[ContextKey];
        }

        private SiteInfo GetSiteInfo()
        {
            var site = _siteFacade.GetSiteSettings("Default").Result;

            return new SiteInfo
            {
                Id = site.Id,
                Name = site.Name
            };
        }

        private PageInfo GetPageInfo()
        {
            return new PageInfo();
        }

        private UserInfo GetUserInfo()
        {
            return new UserInfo();
        }

        private ThemeInfo GetThemeInfo()
        {
            return new ThemeInfo();
        }

        private LanguageInfo GetLanguageInfo()
        {
            var requestedLanguageId = _httpContextAccessor.HttpContext.GetRouteValue(ContextKeys.LanguageKey);

            var userCulture = _httpContextAccessor.HttpContext.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];

            return new LanguageInfo();
        }
    }
}
