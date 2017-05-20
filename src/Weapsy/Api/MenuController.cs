using System;
using System.Threading.Tasks;
using Weapsy.Mvc.Controllers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Domain.Menus.Commands;
using Weapsy.Domain.Menus;
using Weapsy.Reporting.Menus;
using Weapsy.Domain.Menus.Rules;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Queries;
using Weapsy.Mvc.Context;
using Weapsy.Reporting.Menus.Queries;

namespace Weapsy.Api
{
    [Route("api/[controller]")]
    public class MenuController : BaseAdminController
    {
        private readonly ICommandSender _commandSender;
        private readonly IQueryDispatcher _queryDispatcher;       
        private readonly IMenuRules _menuRules;

        public MenuController(ICommandSender commandSender,
            IQueryDispatcher queryDispatcher,          
            IMenuRules menuRules,
            IContextService contextService)
            : base(contextService)
        {
            _commandSender = commandSender;
            _queryDispatcher = queryDispatcher;
            _menuRules = menuRules;            
        }

        [HttpGet]
        [Route("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            var model = await _queryDispatcher.DispatchAsync<GetViewModel, IEnumerable<MenuViewModel>>(new GetViewModel
            {
                SiteId = SiteId,
                Name = name
            });
            return Ok(model);
        }

        [HttpGet]
        [Route("admin")]
        public async Task<IActionResult> GetAllForAdmin()
        {
            var models = await _queryDispatcher.DispatchAsync<GetAllForAdmin, IEnumerable<MenuAdminModel>>(new GetAllForAdmin
            {
                SiteId = SiteId
            });
            return Ok(models);
        }
        [Route("{id}/items")]
        public async Task<IActionResult> GetItemsForAdmin(Guid id)
        {
            var models = await _queryDispatcher.DispatchAsync<GetItemsForAdmin, IEnumerable<MenuItemAdminListModel>>(new GetItemsForAdmin
            {
                SiteId = SiteId,
                Id = id
            });
            return Ok(models);
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
        [Route("admin-list")]
        public async Task<IActionResult> AdminList()
        {
            var models = await _queryDispatcher.DispatchAsync<GetAllForAdmin, IEnumerable<MenuAdminModel>>(new GetAllForAdmin { SiteId = SiteId });
            return Ok(models);
        }

        [HttpGet]
        [Route("{id}/admin-edit")]
        public async Task<IActionResult> AdminEdit(Guid id)
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

        [HttpGet]
        [Route("{id}/admin-edit-item/{itemId}")]
        public async Task<IActionResult> AdminEditItem(Guid id, Guid itemId)
        {
            var model = await _queryDispatcher.DispatchAsync<GetItemForAdmin, MenuItemAdminModel>(new GetItemForAdmin
            {
                SiteId = SiteId,
                MenuId = id,
                MenuItemId = itemId
            });

            if (model == null)
                return NotFound();

            return Ok(model);
        }

        [HttpGet]
        [Route("{id}/admin-edit-default-item")]
        public async Task<IActionResult> AdminEditDefaultItem(Guid id)
        {
            var model = await _queryDispatcher.DispatchAsync<GetDefaultItemForAdmin, MenuItemAdminModel>(new GetDefaultItemForAdmin
            {
                SiteId = SiteId,
                MenuId = id
            });
            return Ok(model);
        }
    }
}
