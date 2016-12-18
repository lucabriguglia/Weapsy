using System;
using Weapsy.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Reporting.Apps;
using Weapsy.Infrastructure.Dispatcher;
using Weapsy.Domain.Apps.Rules;
using Weapsy.Domain.Apps.Commands;
using Weapsy.Domain.Apps;
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
        public IActionResult Get()
        {
            var apps = _appFacade.GetAllForAdmin();
            return Ok(apps);
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var app = _appFacade.GetForAdmin(id);
            if (app == null)
                return NotFound();
            return Ok(app);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateApp model)
        {
            model.Id = Guid.NewGuid();
            _commandSender.Send<CreateApp, App>(model);
            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/update")]
        public IActionResult UpdateDetails([FromBody] UpdateAppDetails model)
        {
            _commandSender.Send<UpdateAppDetails, App>(model);
            return new NoContentResult();
        }

        [HttpGet("{name}")]
        [Route("isAppNameUnique")]
        public IActionResult IsAppNameUnique(string name)
        {
            var isAppNameUnique = _appRules.IsAppNameUnique(name);
            return Ok(isAppNameUnique);
        }

        [HttpGet("{folder}")]
        [Route("isAppFolderUnique")]
        public IActionResult IsAppFolderUnique(string folder)
        {
            var isAppFolderUnique = _appRules.IsAppFolderUnique(folder);
            return Ok(isAppFolderUnique);
        }

        [HttpGet("{folder}")]
        [Route("isAppFolderValid")]
        public IActionResult IsAppFolderValid(string folder)
        {
            var isAppFolderValid = _appRules.IsAppFolderValid(folder);
            return Ok(isAppFolderValid);
        }

        [HttpGet]
        [Route("{id}/admin-list")]
        public IActionResult AdminList()
        {
            var model = _appFacade.GetAllForAdmin();
            return Ok(model);
        }

        [HttpGet]
        [Route("{id}/admin-edit")]
        public IActionResult AdminEdit(Guid id)
        {
            var model = _appFacade.GetForAdmin(id);
            if (model == null)
                return NotFound();
            return Ok(model);
        }
    }
}
