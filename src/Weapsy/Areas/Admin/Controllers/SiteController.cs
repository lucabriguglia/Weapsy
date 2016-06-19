using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;
using Weapsy.Reporting.Sites;

namespace Weapsy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SiteController : BaseAdminController
    {
        private readonly ISiteFacade _siteFacade;

        public SiteController(ISiteFacade siteFacade,
            IContextService contextService)
            : base(contextService)
        {
            _siteFacade = siteFacade;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Edit()
        {
            var model = await _siteFacade.GetAdminModel(SiteId);
            if (model == null) return NotFound();
            return View(model);
        }
    }
}
