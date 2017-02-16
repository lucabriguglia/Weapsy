using Weapsy.Mvc.Controllers;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Reporting.ModuleTypes;
using Weapsy.Mvc.Context;
using Weapsy.Domain.ModuleTypes.Commands;
using Weapsy.Domain.ModuleTypes;
using Weapsy.Infrastructure.Commands;
using Weapsy.Infrastructure.Queries;
using Weapsy.Reporting.ModuleTypes.Queries;

namespace Weapsy.Areas.Admin.Controllers
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
            var model = await _queryDispatcher.DispatchAsync<GetAllForAdmin, IEnumerable<ModuleTypeAdminListModel>>(new GetAllForAdmin());
            return Ok(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = await _queryDispatcher.DispatchAsync<GetDefaultForAdmin, ModuleTypeAdminModel>(new GetDefaultForAdmin());
            return Ok(model);
        }

        public async Task<IActionResult> Save(CreateModuleType model)
        {
            model.Id = Guid.NewGuid();
            await Task.Run(() => _commandSender.Send<CreateModuleType, ModuleType>(model));
            return new NoContentResult();
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await _queryDispatcher.DispatchAsync<GetForAdmin, ModuleTypeAdminModel>(new GetForAdmin
            {
                Id = id
            });

            if (model == null)
                return NotFound();

            return Ok(model);
        }

        public async Task<IActionResult> Update(UpdateModuleTypeDetails model)
        {
            await Task.Run(() => _commandSender.Send<UpdateModuleTypeDetails, ModuleType>(model));
            return new NoContentResult();
        }
    }
}
