using Microsoft.AspNetCore.Http;
using Weapsy.Reporting.Sites;
using Weapsy.Reporting.Themes;
using Weapsy.Reporting.Languages;
using System;

namespace Weapsy.Mvc.Context
{
    public class ContextService : IContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISiteFacade _siteFacade;
        private readonly ILanguageFacade _languageFacade;
        private readonly IThemeFacade _themeFacade;

        public ContextService(IHttpContextAccessor httpContextAccessor, 
            ISiteFacade siteFacade,
            ILanguageFacade languageFacade,
            IThemeFacade themeFacade)
        {
            _httpContextAccessor = httpContextAccessor;
            _siteFacade = siteFacade;
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

            return new ContextInfo
            {
                Site = site,
                Language = language
            };
        }

        private SiteInfo GetSiteInfo()
        {
            return _siteFacade.GetSiteInfo("Default").Result;
        }

        private LanguageInfo GetLanguageInfo()
        {
            //var requestedLanguageId = GetIdFromRouteData(ContextKeys.LanguageKey);
            //var userCulture = _httpContextAccessor.HttpContext.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];

            return new LanguageInfo
            {
                Id = new Guid()
            };
        }

        //private Guid GetIdFromRouteData(string key)
        //{
        //    return _httpContextAccessor.HttpContext.GetRouteData().Values[key] != null
        //        ? (Guid)_httpContextAccessor.HttpContext.GetRouteData().Values[key]
        //        : Guid.Empty;
        //}
    }
}
