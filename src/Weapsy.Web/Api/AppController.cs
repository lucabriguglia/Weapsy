using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Cqrs;
using Weapsy.Domain.Apps;
using Weapsy.Domain.Apps.Commands;
using Weapsy.Domain.Apps.Rules;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;
using Weapsy.Reporting.Apps;
using Weapsy.Reporting.Apps.Queries;

namespace Weapsy.Web.Api
{
    [Route("api/[controller]")]
    public class AppController : BaseAdminController
    {
        private readonly IDispatcher _dispatcher;
        private readonly IAppRules _appRules;

        public AppController(IDispatcher dispatcher,
            IAppRules appRules,
            IContextService contextService)
            : base(contextService)
        {
            _dispatcher = dispatcher;
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
            _dispatcher.SendAndPublish<CreateApp, App>(model);
            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/update")]
        public IActionResult UpdateDetails([FromBody] UpdateAppDetails model)
        {
            _dispatcher.SendAndPublish<UpdateAppDetails, App>(model);
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
            var model = await _dispatcher.GetResultAsync<GetAppAdminModelList, IEnumerable<AppAdminListModel>>(new GetAppAdminModelList());
            return Ok(model);
        }

        [HttpGet]
        [Route("{id}/admin-edit")]
        public async Task<IActionResult> AdminEdit(Guid id)
        {
            var model = await _dispatcher.GetResultAsync<GetAppAdminModel, AppAdminModel>(new GetAppAdminModel {Id = id});

            if (model == null)
                return NotFound();

            return Ok(model);
        }
    }
}
