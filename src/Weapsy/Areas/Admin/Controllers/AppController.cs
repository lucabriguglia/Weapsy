using System;
using Weapsy.Mvc.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Reporting.Apps;
using Weapsy.Domain.Apps;
using Weapsy.Domain.Apps.Commands;
using Weapsy.Infrastructure.Dispatcher;
using Weapsy.Mvc.Context;

namespace Weapsy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AppController : BaseAdminController
    {
        private readonly IAppFacade _appFacade;
        private readonly ICommandSender _commandSender;

        public AppController(IAppFacade appFacade,
            ICommandSender commandSender,
            IContextService contextService)
            : base(contextService)
        {
            _appFacade = appFacade;
            _commandSender = commandSender;
        }

        public async Task<IActionResult> Index()
        {
            var model = await Task.Run(() => _appFacade.GetAllForAdmin());
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = await Task.Run(() => _appFacade.GetDefaultAdminModel());
            return View(model);
        }

        public async Task<IActionResult> Save(CreateApp model)
        {
            model.Id = Guid.NewGuid();
            await Task.Run(() => _commandSender.Send<CreateApp, App>(model));
            return new NoContentResult();
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await Task.Run(() => _appFacade.GetAdminModel(id));
            if (model == null) return NotFound();
            return View(model);
        }

        public async Task<IActionResult> Update(UpdateAppDetails model)
        {
            await Task.Run(() => _commandSender.Send<UpdateAppDetails, App>(model));
            return new NoContentResult();
        }
    }
}
