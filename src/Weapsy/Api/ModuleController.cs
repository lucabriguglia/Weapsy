using Microsoft.AspNetCore.Mvc;
using System;
using Weapsy.Domain.Modules.Commands;
using Weapsy.Domain.Modules.Rules;
using Weapsy.Framework.Commands;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;

namespace Weapsy.Api
{
    [Route("api/[controller]")]
    public class ModuleController : BaseAdminController
    {
        private readonly ICommandSender _commandSender;
        private readonly IModuleRules _moduleRules;

        public ModuleController(ICommandSender commandSender,          
            IModuleRules moduleRules,
            IContextService contextService)
            : base(contextService)
        {
            _commandSender = commandSender;         
            _moduleRules = moduleRules;
        }

        [HttpGet]
        public IActionResult Get()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateModule model)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{id}/admin-list")]
        public IActionResult AdminList()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{id}/admin-edit")]
        public IActionResult AdminEdit(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
