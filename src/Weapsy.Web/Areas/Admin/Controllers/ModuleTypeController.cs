using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Cqrs;
using Weapsy.Domain.ModuleTypes;
using Weapsy.Domain.ModuleTypes.Commands;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;
using Weapsy.Reporting.Apps;
using Weapsy.Reporting.Apps.Queries;

namespace Weapsy.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ModuleTypeController : BaseAdminController
    {
        private readonly IDispatcher _dispatcher;

        public ModuleTypeController(IDispatcher dispatcher,
            IContextService contextService)
            : base(contextService)
        {
            _dispatcher = dispatcher;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _dispatcher.GetResultAsync<GetModuleTypeAdminListModel, IEnumerable<ModuleTypeAdminListModel>>(new GetModuleTypeAdminListModel());
            return Ok(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = await _dispatcher.GetResultAsync<GetDefaultModuleTypeAdminModel, ModuleTypeAdminModel>(new GetDefaultModuleTypeAdminModel());
            return Ok(model);
        }

        public IActionResult Save(CreateModuleType model)
        {
            model.Id = Guid.NewGuid();
            _dispatcher.SendAndPublish<CreateModuleType, ModuleType>(model);
            return new NoContentResult();
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await _dispatcher.GetResultAsync<GetModuleTypeAdminModel, ModuleTypeAdminModel>(new GetModuleTypeAdminModel
            {
                Id = id
            });

            if (model == null)
                return NotFound();

            return Ok(model);
        }

        public IActionResult Update(UpdateModuleTypeDetails model)
        {
            _dispatcher.SendAndPublish<UpdateModuleTypeDetails, ModuleType>(model);
            return new NoContentResult();
        }
    }
}
