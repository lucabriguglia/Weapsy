using System;
using System.Threading.Tasks;
using Weapsy.Mvc.Controllers;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Domain.Model.Menus.Commands;
using Weapsy.Domain.Model.Menus;
using Weapsy.Reporting.Menus;
using Weapsy.Core.Dispatcher;
using Weapsy.Domain.Model.Menus.Rules;
using Weapsy.Mvc.Context;

namespace Weapsy.Api
{
    [Route("api/[controller]")]
    public class MenuController : BaseAdminController
    {
        private readonly IMenuFacade _menuFacade;
        private readonly ICommandSender _commandSender;
        private readonly IMenuRules _menuRules;

        public MenuController(IMenuFacade menuFacade,
            ICommandSender commandSender,
            IMenuRules menuRules,
            IContextService contextService)
            : base(contextService)
        {
            _menuFacade = menuFacade;
            _commandSender = commandSender;
            _menuRules = menuRules;
        }

        [HttpGet]
        [Route("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            var menu = await _menuFacade.GetByNameAsync(SiteId, name);
            return Ok(menu);
        }

        [HttpGet]
        [Route("admin")]
        public async Task<IActionResult> GetAllForAdmin()
        {
            var menus = await _menuFacade.GetAllForAdminAsync(SiteId);
            return Ok(menus);
        }

        [HttpGet()]
        [Route("{id}/items")]
        public async Task<IActionResult> GetItemsForAdmin(Guid id)
        {
            var menuItems = await _menuFacade.GetMenuItemsForAdminListAsync(SiteId, id);
            return Ok(menuItems);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string name)
        {
            var newMenuId = Guid.NewGuid();
            await Task.Run(() => _commandSender.Send<CreateMenu, Menu>(new CreateMenu
            {
                SiteId = SiteId,
                Id = newMenuId,
                Name = name
            }));
            return Ok(newMenuId);
        }

        [HttpPut]
        [Route("{id}/addItem")]
        public async Task<IActionResult> AddItem(Guid id, [FromBody] AddMenuItem model)
        {
            model.SiteId = SiteId;
            model.MenuId = id;
            model.MenuItemId = Guid.NewGuid();

            await Task.Run(() => _commandSender.Send<AddMenuItem, Menu>(model));

            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/updateItem")]
        public async Task<IActionResult> UpdateItem(Guid id, [FromBody] UpdateMenuItem model)
        {
            model.SiteId = SiteId;
            model.MenuId = id;
            await Task.Run(() => _commandSender.Send<UpdateMenuItem, Menu>(model));
            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/reorder")]
        public async Task<IActionResult> Reorder(Guid id, [FromBody] List<ReorderMenuItems.MenuItem> model)
        {
            var command = new ReorderMenuItems { SiteId = SiteId, Id = id, MenuItems = model };
            await Task.Run(() => _commandSender.Send<ReorderMenuItems, Menu>(command));
            return new NoContentResult();
        }

        [HttpDelete]
        [Route("{id}/item/{itemId}")]
        public async Task<IActionResult> DeleteItem(Guid id, Guid itemId)
        {
            await Task.Run(() => _commandSender.Send<RemoveMenuItem, Menu>(new RemoveMenuItem
            {
                SiteId = SiteId,
                MenuId = id,
                MenuItemId = itemId
            }));
            return new NoContentResult();
        }

        [HttpGet("{name}")]
        [Route("isMenuNameUnique")]
        public IActionResult IsMenuNameUnique(string name)
        {
            var isMenuNameUnique = _menuRules.IsMenuNameUnique(SiteId, name);
            return Ok(isMenuNameUnique);
        }

        [HttpGet("{name}")]
        [Route("isMenuNameValid")]
        public IActionResult IsMenuNameValid(string name)
        {
            var isMenuNameValid = _menuRules.IsMenuNameValid(name);
            return Ok(isMenuNameValid);
        }

        [HttpGet]
        [Route("{id}/admin-list")]
        public async Task<IActionResult> AdminList()
        {
            var model = await _menuFacade.GetAllForAdminAsync(SiteId);
            return Ok(model);
        }

        [HttpGet]
        [Route("{id}/admin-edit")]
        public async Task<IActionResult> AdminEdit(Guid id)
        {
            var model = await _menuFacade.GetForAdminAsync(SiteId, id);
            if (model == null) return NotFound();
            return Ok(model);
        }

        [HttpGet()]
        [Route("{id}/admin-edit-item/{itemId}")]
        public async Task<IActionResult> AdminEditItem(Guid id, Guid itemId)
        {
            var item = await _menuFacade.GetItemForAdminAsync(SiteId, id, itemId);
            return Ok(item);
        }
    }
}
