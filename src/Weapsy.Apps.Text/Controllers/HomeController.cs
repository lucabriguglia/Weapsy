using System;
using System.Threading.Tasks;
using Weapsy.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Apps.Text.Reporting;
using Weapsy.Apps.Text.Domain.Commands;
using Weapsy.Mvc.Context;
using Weapsy.Apps.Text.Domain;
using Weapsy.Cqrs;
using Weapsy.Mvc.Apps;

namespace Weapsy.Apps.Text.Controllers
{
    [App("Weapsy.Apps.Text")]
    public class HomeController : BaseAdminController
    {
        private readonly IDispatcher _dispatcher;

        public HomeController(IDispatcher dispatcher,
            IContextService contextService)
            : base(contextService)
        {
            _dispatcher = dispatcher;
        }

        public async Task<IActionResult> Index(Guid moduleId)
        {
            var model = await _dispatcher.GetResultAsync<GetAdminModel, AddVersion>(new GetAdminModel
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
            _dispatcher.SendAndPublish<AddVersion, TextModule>(model);
            return Ok(string.Empty);
        }
    }
}
