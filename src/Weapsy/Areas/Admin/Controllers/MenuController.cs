using System;
using Weapsy.Mvc.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Reporting.Menus;
using Weapsy.Mvc.Context;
using System.Collections.Generic;

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
            //var allMenus = await _menuFacade.GetAllForAdminAsync(SiteId);
            //if (allMenus.Count() > 0)
            //{
            //    var mainMenuId = allMenus.FirstOrDefault().Id;
            //    var model = await _menuFacade.GetMenuItemsForAdminListAsync(SiteId, mainMenuId);
            //    return View(model);
            //}
            return View(new List<MenuItemAdminListModel>());
        }

        public IActionResult Create()
        {
            var model = new MenuAdminModel();
            return View(model);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await Task.Run(() => _menuFacade.GetForAdmin(SiteId, id));
            if (model == null) return NotFound();
            return View(model);
        }

        public IActionResult Item()
        {
            return View();
        }
    }
}
