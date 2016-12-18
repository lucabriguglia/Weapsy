using System;
using Weapsy.Mvc.Controllers;
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

        public IActionResult Index()
        {
            var model = _appFacade.GetAllForAdmin();
            return View(model);
        }

        public IActionResult Create()
        {
            var model = _appFacade.GetDefaultForAdmin();
            return View(model);
        }

        public IActionResult Save(CreateApp model)
        {
            model.Id = Guid.NewGuid();
            _commandSender.Send<CreateApp, App>(model);
            return new NoContentResult();
        }

        public IActionResult Edit(Guid id)
        {
            var model = _appFacade.GetForAdmin(id);
            if (model == null)
                return NotFound();
            return View(model);
        }

        public IActionResult Update(UpdateAppDetails model)
        {
            _commandSender.Send<UpdateAppDetails, App>(model);
            return new NoContentResult();
        }
    }
}
