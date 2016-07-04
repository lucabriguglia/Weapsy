using Microsoft.AspNetCore.Http;
using Weapsy.Reporting.Sites;
using Weapsy.Reporting.Themes;

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
                SiteId = site.Id,
                SiteName = site.Name
            };
        }
    }
}
