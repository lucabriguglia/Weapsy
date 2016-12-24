using System;
using Weapsy.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Apps.Text.Reporting;
using Weapsy.Apps.Text.Domain.Commands;
using Weapsy.Infrastructure.Dispatcher;
using Weapsy.Mvc.Context;
using Weapsy.Apps.Text.Domain;
using Weapsy.Mvc.Apps;

namespace Weapsy.Apps.Text.Controllers
{
    [App("Weapsy.Apps.Text")]
    public class HomeController : BaseAdminController
    {
        private readonly ITextModuleFacade _textFacade;
        private readonly ICommandSender _commandSender;

        public HomeController(ITextModuleFacade textFacade,
            ICommandSender commandSender,
            IContextService contextService)
            : base(contextService)
        {
            _textFacade = textFacade;
            _commandSender = commandSender;
        }

        public IActionResult Index(Guid moduleId)
        {
            var model = _textFacade.GetAdminModel(SiteId, moduleId);
            return View(model);
        }

        public IActionResult Save(AddVersion model)
        {
            model.SiteId = SiteId;
            model.VersionId = Guid.NewGuid();
            _commandSender.Send<AddVersion, TextModule>(model);
            return Ok(string.Empty);
        }
    }
}
