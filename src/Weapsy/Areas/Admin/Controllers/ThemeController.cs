using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Weapsy.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Domain.Themes;
using Weapsy.Domain.Themes.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Queries;
using Weapsy.Mvc.Context;
using Weapsy.Reporting.Themes;
using Weapsy.Reporting.Themes.Queries;

namespace Weapsy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ThemeController : BaseAdminController
    {
        private readonly ICommandSender _commandSender;
        private readonly IQueryDispatcher _queryDispatcher;

        public ThemeController(ICommandSender commandSender,
            IQueryDispatcher queryDispatcher,
            IContextService contextService)
            : base(contextService)
        {
            _commandSender = commandSender;
            _queryDispatcher = queryDispatcher;
        }


        public async Task<IActionResult> Index()
        {
            var model = await _queryDispatcher.DispatchAsync<GetAllForAdmin, IEnumerable<ThemeAdminModel>>(new GetAllForAdmin());
            return View(model);
        }

        public IActionResult Create()
        {
            return View(new ThemeAdminModel());
        }

        public IActionResult Save(CreateTheme model)
        {
            model.Id = Guid.NewGuid();
            _commandSender.Send<CreateTheme, Theme>(model);
            return new NoContentResult();
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await _queryDispatcher.DispatchAsync<GetForAdmin, ThemeAdminModel>(new GetForAdmin { Id = id });

            if (model == null)
                return NotFound();

            return View(model);
        }

        public IActionResult Update(UpdateThemeDetails model)
        {
            _commandSender.Send<UpdateThemeDetails, Theme>(model);
            return new NoContentResult();
        }
    }
}
