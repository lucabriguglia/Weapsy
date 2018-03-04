using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Cqrs;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;
using Weapsy.Reporting.Languages;
using Weapsy.Reporting.Languages.Queries;

namespace Weapsy.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LanguageController : BaseAdminController
    {
        private readonly IDispatcher _dispatcher;

        public LanguageController(IDispatcher dispatcher,
            IContextService contextService)
            : base(contextService)
        {
            _dispatcher = dispatcher;
        }

        public async Task<IActionResult> Index()
        {
            var models = await _dispatcher.GetResultAsync<GetAllForAdmin, IEnumerable<LanguageAdminModel>>(new GetAllForAdmin { SiteId = SiteId });
            return View(models);
        }

        public IActionResult Create()
        {
            var model = new LanguageAdminModel();
            return View(model);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await _dispatcher.GetResultAsync<GetForAdmin, LanguageAdminModel>(new GetForAdmin { SiteId = SiteId, Id = id });

            if (model == null)
                return NotFound();

            return View(model);
        }
    }
}
