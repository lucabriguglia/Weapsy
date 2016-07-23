using Weapsy.Mvc.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Reporting.Modules;
using Weapsy.Core.Dispatcher;
using Weapsy.Mvc.Context;

namespace Weapsy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ModuleController : BaseAdminController
    {
        private readonly IModuleFacade _moduleFacade;
        private readonly ICommandSender _commandSender;

        public ModuleController(IModuleFacade moduleFacade,
            ICommandSender commandSender,
            IContextService contextService)
            : base(contextService)
        {
            _moduleFacade = moduleFacade;
            _commandSender = commandSender;
        }

        public IActionResult Settings()
        {
            return View();
        }

        public IActionResult Permissions()
        {
            return View();
        }

        //public async Task<IActionResult> Create()
        //{
        //    var model = await Task.Run(() => _moduleFacade.GetDefaultAdminModel());
        //    return View(model);
        //}

        //public async Task<IActionResult> Save(CreateModule model)
        //{
        //    model.Id = Guid.NewGuid();
        //    await Task.Run(() => _commandSender.Send<CreateModule, Module>(model));
        //    return Ok(string.Empty);
        //}

        //public async Task<IActionResult> Edit(Guid id)
        //{
        //    var model = await Task.Run(() => _moduleFacade.GetAdminModel(id));
        //    if (model == null) return NotFound();
        //    return View(model);
        //}

        //public async Task<IActionResult> Update(UpdateModuleDetails model)
        //{
        //    await Task.Run(() => _commandSender.Send<UpdateModuleDetails, Module>(model));
        //    return Ok(string.Empty);
        //}
    }
}
