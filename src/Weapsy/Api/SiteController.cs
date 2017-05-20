using System;
using Weapsy.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Reporting.Sites;
using Weapsy.Domain.Sites.Rules;
using Weapsy.Domain.Sites.Commands;
using Weapsy.Domain.Sites;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Queries;
using Weapsy.Mvc.Context;
using Weapsy.Reporting.Sites.Queries;

namespace Weapsy.Api
{
    [Route("api/[controller]")]
    public class SiteController : BaseAdminController
    {
        private readonly ICommandSender _commandSender;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ISiteRules _siteRules;

        public SiteController(ICommandSender commandSender,
            IQueryDispatcher queryDispatcher,
            ISiteRules siteRules,
            IContextService contextService)
            : base(contextService)
        {
            _commandSender = commandSender;
            _queryDispatcher = queryDispatcher;
            _siteRules = siteRules;
            
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
        public IActionResult Post([FromBody] CreateSite model)
        {
            model.Id = Guid.NewGuid();
            _commandSender.Send<CreateSite, Site>(model);
            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/update")]
        public IActionResult UpdateDetails([FromBody] UpdateSiteDetails model)
        {
            _commandSender.Send<UpdateSiteDetails, Site>(model);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _commandSender.Send<DeleteSite, Site>(new DeleteSite { Id = id });
            return new NoContentResult();
        }

        [HttpGet("{name}")]
        [Route("isSiteNameUnique")]
        public IActionResult IsSiteNameUnique(string name)
        {
            var isSiteNameUnique = _siteRules.IsSiteNameUnique(name);
            return Ok(isSiteNameUnique);
        }

        [HttpGet]
        [Route("{id}/admin-edit")]
        public IActionResult AdminEdit(Guid id)
        {
            var model = _queryDispatcher.DispatchAsync<GetAdminModel, SiteAdminModel>(new GetAdminModel { Id = id });

            if (model == null)
                return NotFound();

            return Ok(model);
        }
    }
}
