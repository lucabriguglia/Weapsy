using System;
using System.Collections.Generic;
using Weapsy.Mvc.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Framework.Queries;
using Weapsy.Reporting.Languages;
using Weapsy.Mvc.Context;
using Weapsy.Reporting.Languages.Queries;

namespace Weapsy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LanguageController : BaseAdminController
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public LanguageController(IQueryDispatcher queryDispatcher,
            IContextService contextService)
            : base(contextService)
        {
            _queryDispatcher = queryDispatcher;
        }

        public async Task<IActionResult> Index()
        {
            var models = await _queryDispatcher.DispatchAsync<GetAllForAdmin, IEnumerable<LanguageAdminModel>>(new GetAllForAdmin { SiteId = SiteId });
            return View(models);
        }

        public IActionResult Create()
        {
            var model = new LanguageAdminModel();
            return View(model);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await _queryDispatcher.DispatchAsync<GetForAdmin, LanguageAdminModel>(new GetForAdmin { SiteId = SiteId, Id = id });

            if (model == null)
                return NotFound();

            return View(model);
        }
    }
}
