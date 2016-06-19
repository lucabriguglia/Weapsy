using System;
using Weapsy.Mvc.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Reporting.Pages;
using Weapsy.Domain.Model.Pages;
using Weapsy.Domain.Model.Pages.Commands;
using Weapsy.Core.Dispatcher;
using Weapsy.Mvc.Context;

namespace Weapsy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PageController : BaseAdminController
    {
        private readonly IPageFacade _pageFacade;
        private readonly ICommandSender _commandSender;

        public PageController(IPageFacade pageFacade,
            ICommandSender commandSender,
            IContextService contextService)
            : base(contextService)
        {
            _pageFacade = pageFacade;
            _commandSender = commandSender;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _pageFacade.GetAllForAdminAsync(SiteId);
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = await _pageFacade.GetDefaultAdminModelAsync(SiteId);
            return View(model);
        }

        public async Task<IActionResult> Save(CreatePage model)
        {
            model.SiteId = SiteId;
            model.Id = Guid.NewGuid();
            await Task.Run(() => _commandSender.Send<CreatePage, Page>(model));
            return Ok(string.Empty);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await _pageFacade.GetAdminModelAsync(SiteId, id);
            if (model == null) return NotFound();
            return View(model);
        }

        public async Task<IActionResult> Update(UpdatePageDetails model)
        {
            model.SiteId = SiteId;
            await Task.Run(() => _commandSender.Send<UpdatePageDetails, Page>(model));
            return Ok(string.Empty);
        }
    }
}
