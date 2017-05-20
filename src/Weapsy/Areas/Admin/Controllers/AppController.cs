using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Weapsy.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Domain.Apps;
using Weapsy.Domain.Apps.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Queries;
using Weapsy.Mvc.Context;
using Weapsy.Reporting.Apps;
using Weapsy.Reporting.Apps.Queries;

namespace Weapsy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AppController : BaseAdminController
    {
        private readonly ICommandSender _commandSender;
        private readonly IQueryDispatcher _queryDispatcher;

        public AppController(ICommandSender commandSender,
            IQueryDispatcher queryDispatcher,
            IContextService contextService)
            : base(contextService)
        {
            _commandSender = commandSender;
            _queryDispatcher = queryDispatcher;
        }


        public async Task<IActionResult> Index()
        {
            var model = await _queryDispatcher.DispatchAsync<GetAppAdminModelList, IEnumerable<AppAdminListModel>>(new GetAppAdminModelList());
            return View(model);
        }

        public IActionResult Create()
        {
            return View(new AppAdminModel());
        }

        public IActionResult Save(CreateApp model)
        {
            model.Id = Guid.NewGuid();
            _commandSender.Send<CreateApp, App>(model);
            return new NoContentResult();
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await _queryDispatcher.DispatchAsync<GetAppAdminModel, AppAdminModel>(new GetAppAdminModel { Id = id });

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
