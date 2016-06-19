using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Weapsy.Core.Dispatcher;
using Weapsy.Domain.Model.Modules;
using Weapsy.Domain.Model.Modules.Commands;
using Weapsy.Domain.Model.Modules.Rules;
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
            model.Id = Guid.NewGuid();
            await Task.Run(() => _commandSender.Send<CreateModule, Module>(model));
            return Ok(string.Empty);
        }

        //[HttpPut]
        //[Route("{id}/update")]
        //public async Task<IActionResult> UpdateDetails([FromBody] UpdateModuleDetails model)
        //{
        //    model.SiteId = SiteId;
        //    await Task.Run(() => _commandSender.Send<UpdateModuleDetails, Module>(model));
        //    return Ok(string.Empty);
        //}

        //[HttpPut]
        //[Route("{id}/activate")]
        //public async Task<IActionResult> Activate(Guid id)
        //{
        //    await Task.Run(() => _commandSender.Send<ActivateModule, Module>(new ActivateModule
        //    {
        //        SiteId = SiteId,
        //        Id = id
        //    }));
        //    return Ok(string.Empty);
        //}

        //[HttpPut]
        //[Route("{id}/hide")]
        //public async Task<IActionResult> Hide(Guid id)
        //{
        //    await Task.Run(() => _commandSender.Send<HideModule, Module>(new HideModule
        //    {
        //        SiteId = SiteId,
        //        Id = id
        //    }));
        //    return Ok(string.Empty);
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(Guid id)
        //{
        //    await Task.Run(() => _commandSender.Send<DeleteModule, Module>(new DeleteModule
        //    {
        //        SiteId = SiteId,
        //        Id = id
        //    }));
        //    return Ok(string.Empty);
        //}

        [HttpGet]
        [Route("{id}/admin-list")]
        public async Task<IActionResult> AdminList()
        {
            var model = await _moduleFacade.GetAllForAdminAsync();
            return Ok(model);
        }

        [HttpGet]
        [Route("{id}/admin-edit")]
        public async Task<IActionResult> AdminEdit(Guid id)
        {
            var model = await _moduleFacade.GetAdminModelAsync(id);
            if (model == null) return NotFound();
            return Ok(model);
        }
    }
}
