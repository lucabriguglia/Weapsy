using System;
using System.Threading.Tasks;
using Weapsy.Mvc.Controllers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Domain.Menus.Commands;
using Weapsy.Domain.Menus;
using Weapsy.Reporting.Menus;
using Weapsy.Infrastructure.Dispatcher;
using Weapsy.Domain.Menus.Rules;
using Weapsy.Mvc.Context;

namespace Weapsy.Api
{
    [Route("api/[controller]")]
    public class MenuController : BaseAdminController
    {
        private readonly ICommandSender _commandSender;
        private readonly IMenuFacade _menuFacade;        
        private readonly IMenuRules _menuRules;

        public MenuController(ICommandSender commandSender, 
            IMenuFacade menuFacade,            
            IMenuRules menuRules,
            IContextService contextService)
            : base(contextService)
        {
            _commandSender = commandSender;
            _menuFacade = menuFacade;            
            _menuRules = menuRules;
        }

        [HttpGet]
        [Route("{name}")]
        public IActionResult Get(string name)
        {
            var menu = _menuFacade.GetByName(SiteId, name);
            return Ok(menu);
        }

        [HttpGet]
        [Route("admin")]
        public IActionResult GetAllForAdmin()
        {
            var menus = _menuFacade.GetAllForAdmin(SiteId);
            return Ok(menus);
        }

        [HttpGet()]
        [Route("{id}/items")]
        public IActionResult GetItemsForAdmin(Guid id)
        {
            var menuItems = _menuFacade.GetMenuItemsForAdminList(SiteId, id);
            return Ok(menuItems);
        }

        [HttpPost]
        public IActionResult Post([FromBody] string name)
        {
            var newMenuId = Guid.NewGuid();
            _commandSender.Send<CreateMenu, Menu>(new CreateMenu
            {
                SiteId = SiteId,
                Id = newMenuId,
                Name = name
            });
            return Ok(newMenuId);
        }

        [HttpPut]
        [Route("{id}/addItem")]
        public IActionResult AddItem(Guid id, [FromBody] AddMenuItem model)
        {
            model.SiteId = SiteId;
            model.MenuId = id;
            model.MenuItemId = Guid.NewGuid();

            _commandSender.Send<AddMenuItem, Menu>(model);

            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/updateItem")]
        public IActionResult UpdateItem(Guid id, [FromBody] UpdateMenuItem model)
        {
            model.SiteId = SiteId;
            model.MenuId = id;
            _commandSender.Send<UpdateMenuItem, Menu>(model);
            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/reorder")]
        public IActionResult Reorder(Guid id, [FromBody] List<ReorderMenuItems.MenuItem> model)
        {
            var command = new ReorderMenuItems { SiteId = SiteId, Id = id, MenuItems = model };
            _commandSender.Send<ReorderMenuItems, Menu>(command);
            return new NoContentResult();
        }

        [HttpDelete]
        [Route("{id}/item/{itemId}")]
        public IActionResult DeleteItem(Guid id, Guid itemId)
        {
            _commandSender.Send<RemoveMenuItem, Menu>(new RemoveMenuItem
            {
                SiteId = SiteId,
                MenuId = id,
                MenuItemId = itemId
            });
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
        public IActionResult AdminList()
        {
            var model = _menuFacade.GetAllForAdmin(SiteId);
            return Ok(model);
        }

        [HttpGet]
        [Route("{id}/admin-edit")]
        public IActionResult AdminEdit(Guid id)
        {
            var model = _menuFacade.GetForAdmin(SiteId, id);
            if (model == null)
                return NotFound();
            return Ok(model);
        }

        [HttpGet]
        [Route("{id}/admin-edit-item/{itemId}")]
        public IActionResult AdminEditItem(Guid id, Guid itemId)
        {
            var item = _menuFacade.GetItemForAdmin(SiteId, id, itemId);
            return Ok(item);
        }

        [HttpGet]
        [Route("{id}/admin-edit-default-item")]
        public IActionResult AdminEditDefaultItem(Guid id)
        {
            var item = _menuFacade.GetDefaultItemForAdmin(SiteId, id);
            return Ok(item);
        }
    }
}
