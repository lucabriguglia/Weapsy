using Weapsy.Mvc.Controllers;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Reporting.ModuleTypes;
using Weapsy.Mvc.Context;
using Weapsy.Infrastructure.Dispatcher;
using Weapsy.Domain.ModuleTypes.Commands;
using Weapsy.Domain.ModuleTypes;

namespace Weapsy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ModuleTypeController : BaseAdminController
    {
        private readonly IModuleTypeFacade _moduleTypeFacade;
        private readonly ICommandSender _commandSender;

        public ModuleTypeController(IModuleTypeFacade moduleTypeFacade,
            ICommandSender commandSender,
            IContextService contextService)
            : base(contextService)
        {
            _moduleTypeFacade = moduleTypeFacade;
            _commandSender = commandSender;
        }

        public async Task<IActionResult> Index()
        {
            var model = await Task.Run(() => _moduleTypeFacade.GetAllForAdmin());
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = await Task.Run(() => _moduleTypeFacade.GetDefaultAdminModel());
            return View(model);
        }

        public async Task<IActionResult> Save(CreateModuleType model)
        {
            model.Id = Guid.NewGuid();
            await Task.Run(() => _commandSender.Send<CreateModuleType, ModuleType>(model));
            return new NoContentResult();
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await Task.Run(() => _moduleTypeFacade.GetAdminModel(id));
            if (model == null) return NotFound();
            return View(model);
        }

        public async Task<IActionResult> Update(UpdateModuleTypeDetails model)
        {
            await Task.Run(() => _commandSender.Send<UpdateModuleTypeDetails, ModuleType>(model));
            return new NoContentResult();
        }
    }
}
