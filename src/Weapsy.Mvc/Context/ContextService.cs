using Microsoft.AspNetCore.Http;
using Weapsy.Reporting.Sites;
using Weapsy.Reporting.Themes;
using Weapsy.Reporting.Languages;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Localization;
using Weapsy.Cqrs;
using Weapsy.Reporting.Languages.Queries;
using Weapsy.Reporting.Sites.Queries;
using Weapsy.Reporting.Themes.Queries;
using Weapsy.Reporting.Users;
using System.Linq;

namespace Weapsy.Mvc.Context
{
    public class ContextService : IContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDispatcher _dispatcher;

        public ContextService(IHttpContextAccessor httpContextAccessor, 
            IDispatcher dispatcher)
        {
            _httpContextAccessor = httpContextAccessor;
            _dispatcher = dispatcher;           
        }

        private const string SiteInfoKey = "Weapsy|SiteInfo";
        private const string LanguageInfoKey = "Weapsy|LanguageInfo";
        private const string ThemeInfoKey = "Weapsy|ThemeInfo";
        private const string UserInfoKey = "Weapsy|UserInfo";

        public SiteInfo GetCurrentSiteInfo()
        {
            return GetInfo(SiteInfoKey, () => _dispatcher.GetResultAsync<GetSiteInfo, SiteInfo>(new GetSiteInfo { Name = "Default" }).Result);
        }

        public void SetLanguageInfo(LanguageInfo languageInfo)
        {
            SetInfo(LanguageInfoKey, languageInfo);
        }

        public LanguageInfo GetCurrentLanguageInfo()
        {
            return GetInfo(LanguageInfoKey, () =>
            {
                var languages = _dispatcher.GetResultAsync<GetAllActive, IEnumerable<LanguageInfo>>(new GetAllActive { SiteId = GetCurrentSiteInfo().Id }).Result;
                var userCookie = _httpContextAccessor.HttpContext.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];

                if (!string.IsNullOrEmpty(userCookie))
                {
                    var userCulture = userCookie.Split('|')[0].Split('=')[1];

                    var userLanguage = languages.FirstOrDefault(x => x.CultureName == userCulture);

                    if (userLanguage != null)
                        return userLanguage;
                }

                return languages.Any() ? languages.FirstOrDefault() : new LanguageInfo();
            });
        }

        public ThemeInfo GetCurrentThemeInfo()
        {
            return GetInfo(ThemeInfoKey, () =>
            {
                var themes = _dispatcher.GetResultAsync<GetActiveThemes, IEnumerable<ThemeInfo>>(new GetActiveThemes()).Result;

                var theme = themes.FirstOrDefault(x => x.Id == GetCurrentSiteInfo().ThemeId);

                if (theme == null)
                    return themes.FirstOrDefault();

                return theme;
            });
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
