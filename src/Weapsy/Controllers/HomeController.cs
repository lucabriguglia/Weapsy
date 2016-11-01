using Weapsy.Mvc.Controllers;
using System;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Reporting.Sites;
using Weapsy.Reporting.Pages;
using Weapsy.Mvc.Context;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using Weapsy.Services.Identity;

namespace Weapsy.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ISiteFacade _siteFacade;
        private readonly IPageFacade _pageFacade;
        private readonly IUserService _userService;

        public HomeController(ISiteFacade siteFacade, 
            IPageFacade pageFacade,
            IUserService userService,
            IContextService contextService)
            : base(contextService)
        {
            _siteFacade = siteFacade;
            _pageFacade = pageFacade;
            _userService = userService;
        }

        public IActionResult Index()
        {
            if (PageInfo == null || !_userService.IsUserAuthorized(User, PageInfo.ViewRoles))
                return NotFound();

            ViewBag.Title = PageInfo.Title;
            ViewBag.MetaDescription = PageInfo.MetaDescription;
            ViewBag.MetaKeywords = PageInfo.MetaKeywords;

            return View(PageInfo);
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
