using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Weapsy.Reporting.Sites;
using Weapsy.Reporting.Themes;
using Microsoft.AspNetCore.Localization;
using Weapsy.Reporting.Languages;
using Weapsy.Reporting.Pages;
using System;
using System.Linq;

namespace Weapsy.Mvc.Context
{
    public class ContextService : IContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISiteFacade _siteFacade;
        private readonly IPageFacade _pageFacade;
        private readonly ILanguageFacade _languageFacade;
        private readonly IThemeFacade _themeFacade;

        public ContextService(IHttpContextAccessor httpContextAccessor, 
            ISiteFacade siteFacade,
            IPageFacade pageFacade,
            ILanguageFacade languageFacade,
            IThemeFacade themeFacade)
        {
            _httpContextAccessor = httpContextAccessor;
            _siteFacade = siteFacade;
            _pageFacade = pageFacade;
            _languageFacade = languageFacade;
            _themeFacade = themeFacade;
        }

        private const string ContextInfoKey = "Weapsy|ContextInfo";

        public ContextInfo GetCurrentContextInfo()
        {
            if (_httpContextAccessor.HttpContext.Items[ContextInfoKey] == null)
                _httpContextAccessor.HttpContext.Items.Add(ContextInfoKey, GetContextInfo());

            return (ContextInfo)_httpContextAccessor.HttpContext.Items[ContextInfoKey];
        }

        private ContextInfo GetContextInfo()
        {
            var site = GetSiteInfo();
            var language = GetLanguageInfo();
            var page = GetPageInfo(site.Id, language.Id);

            return new ContextInfo
            {
                Site = site,
                Language = language,
                Page = page
            };
        }

        private SiteInfo GetSiteInfo()
        {
            return _siteFacade.GetSiteInfo("Default").Result;
        }

        private PageInfo GetPageInfo(Guid siteId, Guid languageId)
        {
            Guid pageId = GetIdFromRouteData(ContextKeys.PageKey);

            if (pageId == Guid.Empty)
            {
                // pageId = Site.HomePageId
                var pages = _pageFacade.GetAllForAdminAsync(siteId).Result;
                var homePage = pages.FirstOrDefault(x => x.Name == "Home");
                if (homePage != null)
                    pageId = homePage.Id;
            }

            return _pageFacade.GetPageInfo(siteId, pageId, languageId);
        }

        private LanguageInfo GetLanguageInfo()
        {
            var requestedLanguageId = GetIdFromRouteData(ContextKeys.LanguageKey);

            //var userCulture = _httpContextAccessor.HttpContext.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];

            return new LanguageInfo
            {
                Id = requestedLanguageId
            };
        }

        private Guid GetIdFromRouteData(string key)
        {
            return _httpContextAccessor.HttpContext.GetRouteData().DataTokens[key] != null
                ? (Guid)_httpContextAccessor.HttpContext.GetRouteData().DataTokens[key]
                : Guid.Empty;
        }
    }
}
