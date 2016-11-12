using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Weapsy.Infrastructure.Dispatcher;
using Weapsy.Domain.Modules;
using Weapsy.Domain.Modules.Commands;
using Weapsy.Domain.Modules.Rules;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;
using Weapsy.Reporting.Modules;

namespace Weapsy.Api
{
    [Route("api/[controller]")]
    public class ModuleController : BaseAdminController
    {
        private readonly IModuleFacade _moduleFacade;
        private readonly ICommandSender _commandSender;
        private readonly IModuleRules _moduleRules;
        private readonly IModuleRepository _moduleRepository;

        public ModuleController(IModuleFacade moduleFacade,
            ICommandSender commandSender,
            IModuleRules moduleRules,
            IModuleRepository moduleRepository,
            IContextService contextService)
            : base(contextService)
        {
            _moduleFacade = moduleFacade;
            _commandSender = commandSender;
            _moduleRules = moduleRules;
            _moduleRepository = moduleRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var list = await Task.Run(() => _moduleRepository.GetAll());
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var module = await Task.Run(() => _moduleRepository.GetById(id));
            if (module == null) return NotFound();
            return Ok(module);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateModule model)
        {
            model.SiteId = SiteId;
            await Task.Run(() => _commandSender.Send<CreateModule, Module>(model));
            return new NoContentResult();
        }

        [HttpGet]
        [Route("{id}/admin-list")]
        public async Task<IActionResult> AdminList()
        {
            var model = await Task.Run(() => _moduleFacade.GetAllForAdmin());
            return Ok(model);
        }

        [HttpGet]
        [Route("{id}/admin-edit")]
        public async Task<IActionResult> AdminEdit(Guid id)
        {
            var model = await Task.Run(() => _moduleFacade.GetAdminModel(id));

            if (model == null)
                return NotFound();

            return Ok(model);
        }
    }
}
