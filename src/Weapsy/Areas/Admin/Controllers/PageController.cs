using System;
using System.Collections.Generic;
using Weapsy.Mvc.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Reporting.Pages;
using Weapsy.Domain.Pages;
using Weapsy.Domain.Pages.Commands;
using Weapsy.Mvc.Context;
using AutoMapper;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Queries;
using Weapsy.Reporting.Pages.Queries;

namespace Weapsy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PageController : BaseAdminController
    {
        private readonly ICommandSender _commandSender;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly IMapper _mapper;

        public PageController(ICommandSender commandSender,
            IQueryDispatcher queryDispatcher,
            IMapper mapper,
            IContextService contextService)
            : base(contextService)
        {
            _commandSender = commandSender;
            _queryDispatcher = queryDispatcher;
            _mapper = mapper;            
        }

        public async Task<IActionResult> Index()
        {
            var model = await _queryDispatcher.DispatchAsync<GetAllForAdmin, IEnumerable<PageAdminListModel>>(new GetAllForAdmin
            {
                SiteId = SiteId
            });
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = await _queryDispatcher.DispatchAsync<GetDefaultForAdmin, PageAdminModel>(new GetDefaultForAdmin
            {
                SiteId = SiteId
            });

            return View(model);
        }

        public IActionResult Save(PageAdminModel model)
        {
            var command = _mapper.Map<CreatePage>(model);
            command.SiteId = SiteId;
            command.Id = Guid.NewGuid();
            command.PagePermissions = model.PagePermissions.ToDomain();
            command.MenuIds = model.Menus.ToCommand();
            _commandSender.Send<CreatePage, Page>(command);
            return new NoContentResult();
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await _queryDispatcher.DispatchAsync<GetForAdmin, PageAdminModel>(new GetForAdmin
            {
                SiteId = SiteId,
                Id = id
            });

            if (model == null)
                return NotFound();

            return View(model);
        }

        public IActionResult Update(PageAdminModel model)
        {
            var command = _mapper.Map<UpdatePageDetails>(model);
            command.SiteId = SiteId;
            command.PagePermissions = model.PagePermissions.ToDomain();
            _commandSender.Send<UpdatePageDetails, Page>(command);
            return new NoContentResult();
        }

        public async Task<IActionResult> EditModule(Guid pageId, Guid pageModuleId)
        {
            var model = await _queryDispatcher.DispatchAsync<GetPageModuleAdminModel, PageModuleAdminModel>(new GetPageModuleAdminModel
            {
                SiteId = SiteId,
                PageId = pageId,
                PageModuleId = pageModuleId
            });

            if (model == null)
                return NotFound();

            return View(model);
        }

        public IActionResult UpdateModule(PageModuleAdminModel model)
        {
            var command = _mapper.Map<UpdatePageModuleDetails>(model);
            command.SiteId = SiteId;
            command.PageModulePermissions = model.PageModulePermissions.ToDomain();
            _commandSender.Send<UpdatePageModuleDetails, Page>(command);
            return new NoContentResult();
        }
    }
}
