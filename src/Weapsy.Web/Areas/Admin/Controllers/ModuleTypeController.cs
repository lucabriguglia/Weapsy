using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Domain.ModuleTypes;
using Weapsy.Domain.ModuleTypes.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Queries;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;
using Weapsy.Reporting.Apps;
using Weapsy.Reporting.Apps.Queries;

namespace Weapsy.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ModuleTypeController : BaseAdminController
    {
        private readonly ICommandSender _commandSender;
        private readonly IQueryDispatcher _queryDispatcher;

        public ModuleTypeController(ICommandSender commandSender,
            IQueryDispatcher queryDispatcher,
            IContextService contextService)
            : base(contextService)
        {
            _commandSender = commandSender;
            _queryDispatcher = queryDispatcher;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _queryDispatcher.DispatchAsync<GetModuleTypeAdminListModel, IEnumerable<ModuleTypeAdminListModel>>(new GetModuleTypeAdminListModel());
            return Ok(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = await _queryDispatcher.DispatchAsync<GetDefaultModuleTypeAdminModel, ModuleTypeAdminModel>(new GetDefaultModuleTypeAdminModel());
            return Ok(model);
        }

        public IActionResult Save(CreateModuleType model)
        {
            model.Id = Guid.NewGuid();
            _commandSender.Send<CreateModuleType, ModuleType>(model);
            return new NoContentResult();
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await _queryDispatcher.DispatchAsync<GetModuleTypeAdminModel, ModuleTypeAdminModel>(new GetModuleTypeAdminModel
            {
                Id = id
            });

            if (model == null)
                return NotFound();

            return Ok(model);
        }

        public IActionResult Update(UpdateModuleTypeDetails model)
        {
            _commandSender.Send<UpdateModuleTypeDetails, ModuleType>(model);
            return new NoContentResult();
        }
    }
}
