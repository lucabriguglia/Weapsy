using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Cqrs;
using Weapsy.Domain.ModuleTypes;
using Weapsy.Domain.ModuleTypes.Commands;
using Weapsy.Domain.ModuleTypes.Rules;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;
using Weapsy.Reporting.Apps;
using Weapsy.Reporting.Apps.Queries;

namespace Weapsy.Web.Api
{
    [Route("api/[controller]")]
    public class ModuleTypeController : BaseAdminController
    {
        private readonly IDispatcher _dispatcher;
        private readonly IModuleTypeRules _moduleTypeRules;

        public ModuleTypeController(IDispatcher dispatcher,          
            IModuleTypeRules moduleTypeRules,
            IContextService contextService)
            : base(contextService)
        {
            _dispatcher = dispatcher;
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
            _dispatcher.SendAndPublish<CreateModuleType, ModuleType>(model);
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
            var model = await _dispatcher.GetResultAsync<GetModuleTypeAdminListModel, IEnumerable<ModuleTypeAdminListModel>>(new GetModuleTypeAdminListModel
            {
                AppId = appId
            });

            return Ok(model);
        }

        [HttpGet]
        [Route("{id}/admin-edit")]
        public async Task<IActionResult> AdminEdit(Guid id)
        {
            var model = await _dispatcher.GetResultAsync<GetModuleTypeAdminModel, ModuleTypeAdminModel>(new GetModuleTypeAdminModel
            {
                Id = id
            });

            if (model == null)
                return NotFound();

            return Ok(model);
        }
    }
}
