using Microsoft.AspNetCore.Mvc;
using System;
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
        private readonly ICommandSender _commandSender;
        private readonly IModuleTypeFacade _moduleTypeFacade;        
        private readonly IModuleTypeRules _moduleTypeRules;

        public ModuleTypeController(ICommandSender commandSender,
            IModuleTypeFacade moduleTypeFacade,            
            IModuleTypeRules moduleTypeRules,
            IContextService contextService)
            : base(contextService)
        {
            _commandSender = commandSender;
            _moduleTypeFacade = moduleTypeFacade;            
            _moduleTypeRules = moduleTypeRules;
        }

        [HttpGet]
        public IActionResult Get()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateModuleType model)
        {
            _commandSender.Send<CreateModuleType, ModuleType>(model);
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
        public IActionResult AdminList()
        {
            var model = _moduleTypeFacade.GetAllForAdmin();
            return Ok(model);
        }

        [HttpGet]
        [Route("{id}/admin-edit")]
        public IActionResult AdminEdit(Guid id)
        {
            var model = _moduleTypeFacade.GetAdminModel(id);
            if (model == null)
                return NotFound();
            return Ok(model);
        }
    }
}
