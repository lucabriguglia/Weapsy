using System;
using System.Threading.Tasks;
using Weapsy.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Apps.Text.Reporting;
using Weapsy.Apps.Text.Domain.Commands;
using Weapsy.Mvc.Context;
using Weapsy.Apps.Text.Domain;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Queries;
using Weapsy.Mvc.Apps;

namespace Weapsy.Apps.Text.Controllers
{
    [App("Weapsy.Apps.Text")]
    public class HomeController : BaseAdminController
    {
        private readonly ICommandSender _commandSender;
        private readonly IQueryDispatcher _queryDispatcher;

        public HomeController(ICommandSender commandSender,
            IQueryDispatcher queryDispatcher,
            IContextService contextService)
            : base(contextService)
        {
            _commandSender = commandSender;
            _queryDispatcher = queryDispatcher;
        }

        public async Task<IActionResult> Index(Guid moduleId)
        {
            var model = await _queryDispatcher.DispatchAsync<GetAdminModel, AddVersion>(new GetAdminModel
            {
                SiteId = SiteId,
                ModuleId = moduleId,
            });

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
