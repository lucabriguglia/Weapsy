using System;
using System.Threading.Tasks;
using Weapsy.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Reporting.Apps;
using Weapsy.Core.Dispatcher;
using Weapsy.Domain.Model.Apps.Rules;
using Weapsy.Domain.Model.Apps.Commands;
using Weapsy.Domain.Model.Apps;
using Weapsy.Mvc.Context;

namespace Weapsy.Api
{
    [Route("api/[controller]")]
    public class AppController : BaseAdminController
    {
        private readonly IAppFacade _appFacade;
        private readonly ICommandSender _commandSender;
        private readonly IAppRules _appRules;

        public AppController(IAppFacade appFacade,
            ICommandSender commandSender,
            IAppRules appRules,
            IContextService contextService)
            : base(contextService)
        {
            _appFacade = appFacade;
            _commandSender = commandSender;
            _appRules = appRules;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var apps = await Task.Run(() => _appFacade.GetAllForAdmin());
            return Ok(apps);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var app = await Task.Run(() => _appFacade.GetAdminModel(id));
            if (app == null) return NotFound();
            return Ok(app);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateApp model)
        {
            model.Id = Guid.NewGuid();
            await Task.Run(() => _commandSender.Send<CreateApp, App>(model));
            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/update")]
        public async Task<IActionResult> UpdateDetails([FromBody] UpdateAppDetails model)
        {
            await Task.Run(() => _commandSender.Send<UpdateAppDetails, App>(model));
            return new NoContentResult();
        }

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(Guid id)
        //{
        //    await Task.Run(() => _commandSender.Send<DeleteApp, App>(new DeleteApp { Id = id }));
        //    return new NoContentResult();
        //}

        [HttpGet("{name}")]
        [Route("isAppNameUnique")]
        public async Task<IActionResult> IsAppNameUnique(string name)
        {
            var isAppNameUnique = await Task.Run(() => _appRules.IsAppNameUnique(name));
            return Ok(isAppNameUnique);
        }

        [HttpGet("{folder}")]
        [Route("isAppFolderUnique")]
        public async Task<IActionResult> IsAppFolderUnique(string folder)
        {
            var isAppFolderUnique = await Task.Run(() => _appRules.IsAppFolderUnique(folder));
            return Ok(isAppFolderUnique);
        }

        [HttpGet("{folder}")]
        [Route("isAppFolderUnique")]
        public async Task<IActionResult> IsAppFolderValid(string folder)
        {
            var isAppFolderValid = await Task.Run(() => _appRules.IsAppFolderValid(folder));
            return Ok(isAppFolderValid);
        }

        [HttpGet]
        [Route("{id}/admin-list")]
        public async Task<IActionResult> AdminList()
        {
            var model = await Task.Run(() => _appFacade.GetAllForAdmin());
            return Ok(model);
        }

        [HttpGet]
        [Route("{id}/admin-edit")]
        public async Task<IActionResult> AdminEdit(Guid id)
        {
            var model = await Task.Run(() => _appFacade.GetAdminModel(id));
            if (model == null) return NotFound();
            return Ok(model);
        }
    }
}
