using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Weapsy.Domain.ModuleTypes;
using Weapsy.Domain.ModuleTypes.Commands;
using Weapsy.Domain.ModuleTypes.Rules;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Queries;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;
using Weapsy.Reporting.Apps;
using Weapsy.Reporting.Apps.Queries;

namespace Weapsy.Api
{
    [Route("api/[controller]")]
    public class ModuleTypeController : BaseAdminController
    {
        private readonly ICommandSender _commandSender;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly IModuleTypeRules _moduleTypeRules;

        public ModuleTypeController(ICommandSender commandSender,
            IQueryDispatcher queryDispatcher,            
            IModuleTypeRules moduleTypeRules,
            IContextService contextService)
            : base(contextService)
        {
            _commandSender = commandSender;
            _queryDispatcher = queryDispatcher;
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
        [Route("{appId}/admin-list")]
        public async Task<IActionResult> AdminList(Guid appId)
        {
            var model = await _queryDispatcher.DispatchAsync<GetModuleTypeAdminListModel, IEnumerable<ModuleTypeAdminListModel>>(new GetModuleTypeAdminListModel
            {
                AppId = appId
            });

            return Ok(model);
        }

        [HttpGet]
        [Route("{id}/admin-edit")]
        public async Task<IActionResult> AdminEdit(Guid id)
        {
            var model = await _queryDispatcher.DispatchAsync<GetModuleTypeAdminModel, ModuleTypeAdminModel>(new GetModuleTypeAdminModel
            {
                Id = id
            });

            if (model == null)
                return NotFound();

            return Ok(model);
        }
    }
}
