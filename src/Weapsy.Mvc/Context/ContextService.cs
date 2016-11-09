using Microsoft.AspNetCore.Http;
using Weapsy.Reporting.Sites;
using Weapsy.Reporting.Themes;
using Weapsy.Reporting.Languages;
using System;
using Weapsy.Reporting.Users;

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
        private const string SiteInfoKey = "Weapsy|SiteInfo";
        private const string LanguageInfoKey = "Weapsy|LanguageInfo";
        private const string ThemeInfoKey = "Weapsy|ThemeInfo";
        private const string UserInfoKey = "Weapsy|UserInfo";

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
            //var userCulture = _httpContextAccessor.HttpContext.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];

            return new LanguageInfo
            {
                Id = new Guid()
            };
        }

        public SiteInfo GetCurrentSiteInfo()
        {
            return GetInfo(SiteInfoKey, GetSiteInfo);
        }

        public LanguageInfo GetCurrentLanguageInfo()
        {
            return GetInfo(LanguageInfoKey, GetLanguageInfo);
        }

        public ThemeInfo GetCurrentThemeInfo()
        {
            throw new NotImplementedException();
        }

        public UserInfo GetCurrentUserInfo()
        {
            throw new NotImplementedException();
        }

        private T GetInfo<T>(string key, Func<T> acquire)
        {
            if (_httpContextAccessor.HttpContext.Items[key] == null)
                _httpContextAccessor.HttpContext.Items.Add(key, acquire());

            return (T)_httpContextAccessor.HttpContext.Items[key];
        }
    }
}
