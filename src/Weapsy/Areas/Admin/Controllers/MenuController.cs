using System;
using Weapsy.Mvc.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Reporting.Menus;
using Weapsy.Mvc.Context;

namespace Weapsy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuController : BaseAdminController
    {
        private readonly IMenuFacade _menuFacade;

        public MenuController(IMenuFacade menuFacade,
            IContextService contextService)
            : base(contextService)
        {
            _menuFacade = menuFacade;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _menuFacade.GetAllForAdminAsync(SiteId);
            return View(model);
        }

        public IActionResult Create()
        {
            var model = new MenuAdminModel();
            return View(model);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await _menuFacade.GetForAdminAsync(SiteId, id);
            if (model == null) return NotFound();
            return View(model);
        }

        public IActionResult Item()
        {
            return View();
        }
    }
}
