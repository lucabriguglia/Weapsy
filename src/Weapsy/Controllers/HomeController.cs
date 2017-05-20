using Weapsy.Mvc.Controllers;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Reporting.Pages;
using Weapsy.Mvc.Context;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using Weapsy.Domain.Pages;
using Weapsy.Framework.Queries;
using Weapsy.Reporting.Pages.Queries;
using Weapsy.Services.Security;

namespace Weapsy.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ISecurityService _securityService;

        public HomeController(IQueryDispatcher queryDispatcher,
            ISecurityService securityService,
            IContextService contextService)
            : base(contextService)
        {
            _queryDispatcher = queryDispatcher;
            _securityService = securityService;            
        }

        public async Task<IActionResult> Index(Guid pageId, Guid languageId)
        {
            if (pageId == Guid.Empty)
                return NotFound();

            var pageInfo = await _queryDispatcher.DispatchAsync<GetPageInfo, PageInfo>(new GetPageInfo
            {
                SiteId = SiteId,
                PageId = pageId,
                LanguageId = languageId
            });

            if (pageInfo == null || !_securityService.IsUserAuthorized(User, pageInfo.Page.Roles[PermissionType.View]))
                return NotFound();

            ViewBag.Title = pageInfo.Page.Title;
            ViewBag.MetaDescription = pageInfo.Page.MetaDescription;
            ViewBag.MetaKeywords = pageInfo.Page.MetaKeywords;

            return View(pageInfo);
        }

        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(!string.IsNullOrWhiteSpace(returnUrl) ? returnUrl : "/");
        }

        [Route("error/500")]
        public IActionResult Error()
        {
            return View();
        }

        [Route("error/404")]
        public IActionResult PageNotFound()
        {
            return View();
        }

        [Route("error/403")]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
