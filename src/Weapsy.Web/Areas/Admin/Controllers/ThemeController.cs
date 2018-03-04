using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Cqrs;
using Weapsy.Domain.Themes;
using Weapsy.Domain.Themes.Commands;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;
using Weapsy.Reporting.Themes;
using Weapsy.Reporting.Themes.Queries;

namespace Weapsy.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ThemeController : BaseAdminController
    {
        private readonly IDispatcher _dispatcher;

        public ThemeController(IDispatcher dispatcher,
            IContextService contextService)
            : base(contextService)
        {
            _dispatcher = dispatcher;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _dispatcher.GetResultAsync<GetAllForAdmin, IEnumerable<ThemeAdminModel>>(new GetAllForAdmin());
            return View(model);
        }

        public IActionResult Create()
        {
            return View(new ThemeAdminModel());
        }

        public IActionResult Save(CreateTheme model)
        {
            model.Id = Guid.NewGuid();
            _dispatcher.SendAndPublish<CreateTheme, Theme>(model);
            return new NoContentResult();
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await _dispatcher.GetResultAsync<GetForAdmin, ThemeAdminModel>(new GetForAdmin { Id = id });

            if (model == null)
                return NotFound();

            return View(model);
        }

        public IActionResult Update(UpdateThemeDetails model)
        {
            _dispatcher.SendAndPublish<UpdateThemeDetails, Theme>(model);
            return new NoContentResult();
        }
    }
}
