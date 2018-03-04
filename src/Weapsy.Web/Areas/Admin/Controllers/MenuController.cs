using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Cqrs;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;
using Weapsy.Reporting.Menus;
using Weapsy.Reporting.Menus.Queries;

namespace Weapsy.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuController : BaseAdminController
    {
        private readonly IDispatcher _dispatcher;

        public MenuController(IDispatcher dispatcher,
            IContextService contextService)
            : base(contextService)
        {
            _dispatcher = dispatcher;
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
            var model = await _dispatcher.GetResultAsync<GetForAdmin, MenuAdminModel>(new GetForAdmin
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
