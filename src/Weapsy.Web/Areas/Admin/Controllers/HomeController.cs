using Microsoft.AspNetCore.Mvc;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;

namespace Weapsy.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : BaseAdminController
    {
        public HomeController(IContextService contextService)
            : base(contextService)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
