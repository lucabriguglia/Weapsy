using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Weapsy.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Reporting.Apps;
using Weapsy.Domain.Apps.Rules;
using Weapsy.Domain.Apps.Commands;
using Weapsy.Domain.Apps;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Queries;
using Weapsy.Mvc.Context;
using Weapsy.Reporting.Apps.Queries;

namespace Weapsy.Api
{
    [Route("api/[controller]")]
    public class AppController : BaseAdminController
    {
        private readonly ICommandSender _commandSender;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly IAppRules _appRules;

        public AppController(ICommandSender commandSender,
            IQueryDispatcher queryDispatcher,
            IAppRules appRules,
            IContextService contextService)
            : base(contextService)
        {
            _commandSender = commandSender;
            _queryDispatcher = queryDispatcher;
            _appRules = appRules;
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
        [Route("admin-list")]
        public async Task<IActionResult> AdminList()
        {
            var model = await _queryDispatcher.DispatchAsync<GetAppAdminModelList, IEnumerable<AppAdminListModel>>(new GetAppAdminModelList());
            return Ok(model);
        }

        [HttpGet]
        [Route("{id}/admin-edit")]
        public async Task<IActionResult> AdminEdit(Guid id)
        {
            var model = await _queryDispatcher.DispatchAsync<GetAppAdminModel, AppAdminModel>(new GetAppAdminModel {Id = id});

            if (model == null)
                return NotFound();

            return Ok(model);
        }
    }
}
