using System;
using Weapsy.Mvc.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Reporting.Pages;
using Weapsy.Domain.Pages;
using Weapsy.Domain.Pages.Commands;
using Weapsy.Infrastructure.Dispatcher;
using Weapsy.Mvc.Context;
using AutoMapper;

namespace Weapsy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PageController : BaseAdminController
    {
        private readonly IPageFacade _pageFacade;
        private readonly ICommandSender _commandSender;
        private readonly IMapper _mapper;

        public PageController(IPageFacade pageFacade,
            ICommandSender commandSender,
            IMapper mapper,
            IContextService contextService)
            : base(contextService)
        {
            _pageFacade = pageFacade;
            _commandSender = commandSender;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var model = await Task.Run(() => _pageFacade.GetAllForAdmin(SiteId));
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = await Task.Run(() => _pageFacade.GetDefaultAdminModel(SiteId));
            return View(model);
        }

        public async Task<IActionResult> Save(PageAdminModel model)
        {
            var command = _mapper.Map<CreatePage>(model);
            command.SiteId = SiteId;
            command.Id = Guid.NewGuid();
            command.PagePermissions = model.PagePermissions.ToDomain();
            command.MenuIds = model.Menus.ToCommand();
            await Task.Run(() => _commandSender.Send<CreatePage, Page>(command));
            return new NoContentResult();
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await Task.Run(() => _pageFacade.GetAdminModel(SiteId, id));

            if (model == null)
                return NotFound();

            return View(model);
        }

        public async Task<IActionResult> Update(PageAdminModel model)
        {
            var command = _mapper.Map<UpdatePageDetails>(model);
            command.SiteId = SiteId;
            command.PagePermissions = model.PagePermissions.ToDomain();
            await Task.Run(() => _commandSender.Send<UpdatePageDetails, Page>(command));
            return new NoContentResult();
        }

        public async Task<IActionResult> EditModule(Guid pageId, Guid pageModuleId)
        {
            var model = await Task.Run(() => _pageFacade.GetModuleAdminModel(SiteId, pageId, pageModuleId));

            if (model == null)
                return NotFound();

            return View(model);
        }

        public async Task<IActionResult> UpdateModule(PageModuleAdminModel model)
        {
            var command = _mapper.Map<UpdatePageModuleDetails>(model);
            command.SiteId = SiteId;
            command.PageModulePermissions = model.PageModulePermissions.ToDomain();
            await Task.Run(() => _commandSender.Send<UpdatePageModuleDetails, Page>(command));
            return new NoContentResult();
        }
    }
}
