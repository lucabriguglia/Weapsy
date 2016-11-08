using Weapsy.Mvc.Controllers;
using System;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Reporting.Pages;
using Weapsy.Mvc.Context;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using Weapsy.Services.Identity;
using System.Linq;

namespace Weapsy.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IPageFacade _pageFacade;
        private readonly IUserService _userService;

        public HomeController(IPageFacade pageFacade,
            IUserService userService,
            IContextService contextService)
            : base(contextService)
        {
            _pageFacade = pageFacade;
            _userService = userService;
        }

        public IActionResult Index(Guid pageId, Guid languageId)
        {
            if (pageId == Guid.Empty)
            {
                // pageId = Site.HomePageId // todo: set pageId to the home page of current site
                var pages = _pageFacade.GetAllForAdminAsync(SiteId).Result;
                var homePage = pages.FirstOrDefault(x => x.Name == "Home");
                if (homePage != null)
                    pageId = homePage.Id;
            }

            var pageInfo = _pageFacade.GetPageInfo(SiteId, pageId, languageId);

            if (pageInfo == null || !_userService.IsUserAuthorized(User, pageInfo.Page.ViewRoles))
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

            return LocalRedirect(returnUrl);
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
