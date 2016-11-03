using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Weapsy.Infrastructure.Dispatcher;
using Weapsy.Domain.ModuleTypes;
using Weapsy.Domain.ModuleTypes.Commands;
using Weapsy.Domain.ModuleTypes.Rules;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;
using Weapsy.Reporting.ModuleTypes;

namespace Weapsy.Api
{
    [Route("api/[controller]")]
    public class ModuleTypeController : BaseAdminController
    {
        private readonly IModuleTypeFacade _moduleTypeFacade;
        private readonly ICommandSender _commandSender;
        private readonly IModuleTypeRules _moduleTypeRules;

        public ModuleTypeController(IModuleTypeFacade moduleTypeFacade,
            ICommandSender commandSender,
            IModuleTypeRules moduleTypeRules,
            IContextService contextService)
            : base(contextService)
        {
            _moduleTypeFacade = moduleTypeFacade;
            _commandSender = commandSender;
            _moduleTypeRules = moduleTypeRules;
        }

        //[HttpGet]
        //public async Task<IActionResult> Get()
        //{
        //    var list = await Task.Run(() => _moduleTypeRepository.GetAll());
        //    return Ok(list);
        //}

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateModuleType model)
        {
            await Task.Run(() => _commandSender.Send<CreateModuleType, ModuleType>(model));
            return new NoContentResult();
        }

        [HttpGet("{name}")]
        [Route("isModuleTypeNameUnique")]
        public IActionResult IsModuleTypeNameUnique(string name)
        {
            var isModuleTypeNameUnique = _moduleTypeRules.IsModuleTypeNameUnique(name);
            return Ok(isModuleTypeNameUnique);
        }

        [HttpGet("{name}")]
        [Route("isModuleTypeNameValid")]
        public IActionResult IsModuleTypeNameValid(string name)
        {
            var isModuleTypeNameValid = _moduleTypeRules.IsModuleTypeNameValid(name);
            return Ok(isModuleTypeNameValid);
        }

        [HttpGet("{viewComponentName}")]
        [Route("iModuleTypeViewComponentNameUnique")]
        public IActionResult IsModuleTypeViewComponentNameUnique(string name)
        {
            var iModuleTypeViewComponentNameUnique = _moduleTypeRules.IsModuleTypeViewComponentNameUnique(name);
            return Ok(iModuleTypeViewComponentNameUnique);
        }

        [HttpGet]
        [Route("{id}/admin-list")]
        public async Task<IActionResult> AdminList()
        {
            var model = await Task.Run(() => _moduleTypeFacade.GetAllForAdmin());
            return Ok(model);
        }

        [HttpGet]
        [Route("{id}/admin-edit")]
        public async Task<IActionResult> AdminEdit(Guid id)
        {
            var model = await Task.Run(() => _moduleTypeFacade.GetAdminModel(id));
            if (model == null) return NotFound();
            return Ok(model);
        }
    }
}
