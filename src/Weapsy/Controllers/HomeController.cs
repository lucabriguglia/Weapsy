using System.Threading.Tasks;
using Weapsy.Mvc.Controllers;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Reporting.Sites;
using Weapsy.Reporting.Pages;
using Weapsy.Mvc.Context;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;

namespace Weapsy.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ISiteFacade _siteFacade;
        private readonly IPageFacade _pageFacade;


        public HomeController(ISiteFacade siteFacade, 
            IPageFacade pageFacade,
            IContextService contextService)
            : base(contextService)
        {
            _siteFacade = siteFacade;
            _pageFacade = pageFacade;
        }

        public async Task<IActionResult> Index()
        {
            Guid pageId = RouteData.DataTokens.Keys.Count > 0 && RouteData.DataTokens["pageId"] != null
                ? (Guid)RouteData.DataTokens["pageId"]
                : Guid.Empty;

            if (pageId == Guid.Empty)
            {
                // todo: load home page as set in site settings
                var pages = await _pageFacade.GetAllForAdminAsync(SiteId);
                var homePage = pages.FirstOrDefault(x => x.Name == "Home");
                if (homePage != null)
                    pageId = homePage.Id;
            }

            var viewModel = await Task.Run(() => _pageFacade.GetPageViewModel(SiteId, pageId));

            if (viewModel == null)
                return NotFound();

            // todo: check permissions (add method inside UserInfo)

            ViewBag.Title = viewModel.Page.Title;
            ViewBag.MetaDescription = viewModel.Page.MetaDescription;
            ViewBag.MetaKeywords = viewModel.Page.MetaKeywords;

            return View(viewModel);
        }

        [HttpPost]
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
