using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Cqrs;
using Weapsy.Domain.Apps;
using Weapsy.Domain.Apps.Commands;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;
using Weapsy.Reporting.Apps;
using Weapsy.Reporting.Apps.Queries;

namespace Weapsy.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AppController : BaseAdminController
    {
        private readonly IDispatcher _dispatcher;

        public AppController(IDispatcher dispatcher,
            IContextService contextService)
            : base(contextService)
        {
            _dispatcher = dispatcher;
        }


        public async Task<IActionResult> Index()
        {
            var model = await _dispatcher.GetResultAsync<GetAppAdminModelList, IEnumerable<AppAdminListModel>>(new GetAppAdminModelList());
            return View(model);
        }

        public IActionResult Create()
        {
            return View(new AppAdminModel());
        }

        public IActionResult Save(CreateApp model)
        {
            model.Id = Guid.NewGuid();
            _dispatcher.SendAndPublish<CreateApp, App>(model);
            return new NoContentResult();
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await _dispatcher.GetResultAsync<GetAppAdminModel, AppAdminModel>(new GetAppAdminModel { Id = id });

            if (model == null)
                return NotFound();

            return View(model);
        }

        public IActionResult Update(UpdateAppDetails model)
        {
            _dispatcher.SendAndPublish<UpdateAppDetails, App>(model);
            return new NoContentResult();
        }
    }
}
