using System;
using Weapsy.Mvc.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Reporting.Menus;
using Weapsy.Mvc.Context;
using System.Collections.Generic;
using Weapsy.Framework.Queries;
using Weapsy.Reporting.Menus.Queries;

namespace Weapsy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuController : BaseAdminController
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public MenuController(IQueryDispatcher queryDispatcher,
            IContextService contextService)
            : base(contextService)
        {
            _queryDispatcher = queryDispatcher;
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
            var model = await _queryDispatcher.DispatchAsync<GetForAdmin, MenuAdminModel>(new GetForAdmin
            {
                SiteId = SiteId,
                Id = id
            });

            if (model == null)
                return NotFound();

            return Ok(model);
        }

        public IActionResult Item()
        {
            return View();
        }
    }
}
