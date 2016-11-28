using Microsoft.AspNetCore.Http;
using Weapsy.Reporting.Sites;
using Weapsy.Reporting.Themes;
using Weapsy.Reporting.Languages;
using System;
using System.Linq;
using Microsoft.AspNetCore.Localization;
using Weapsy.Domain.Languages;
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

        private const string SiteInfoKey = "Weapsy|SiteInfo";
        private const string LanguageInfoKey = "Weapsy|LanguageInfo";
        private const string ThemeInfoKey = "Weapsy|ThemeInfo";
        private const string UserInfoKey = "Weapsy|UserInfo";

        public SiteInfo GetCurrentSiteInfo()
        {
            return GetInfo(SiteInfoKey, () => _siteFacade.GetSiteInfo("Default"));
        }

        public void SetLanguageInfo(LanguageInfo languageInfo)
        {
            SetInfo(LanguageInfoKey, languageInfo);
        }

        public LanguageInfo GetCurrentLanguageInfo()
        {
            return GetInfo(LanguageInfoKey, () =>
            {
                var languages = _languageFacade.GetAllActiveAsync(GetCurrentSiteInfo().Id).Result;
                var userCoockie = _httpContextAccessor.HttpContext.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];

                if (!string.IsNullOrEmpty(userCoockie))
                {
                    var userCulture = userCoockie.Split('|')[0].Split('=')[1];

                    var userLanguage = languages.FirstOrDefault(x => x.CultureName == userCulture);

                    if (userLanguage != null)
                        return userLanguage;
                }

                return languages.Any() ? languages.FirstOrDefault() : new LanguageInfo();
            });
        }

        public ThemeInfo GetCurrentThemeInfo()
        {
            throw new NotImplementedException();
        }

        public UserInfo GetCurrentUserInfo()
        {
            throw new NotImplementedException();
        }

        private void SetInfo(string key, object data)
        {
            _httpContextAccessor.HttpContext.Items.Add(key, data);
        }

        private T GetInfo<T>(string key, Func<T> acquire)
        {
            if (_httpContextAccessor.HttpContext.Items[key] == null)
                _httpContextAccessor.HttpContext.Items.Add(key, acquire());

            return (T)_httpContextAccessor.HttpContext.Items[key];
        }
    }
}
