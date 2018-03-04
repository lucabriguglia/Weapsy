using System;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Cqrs;
using Weapsy.Domain.Sites;
using Weapsy.Domain.Sites.Commands;
using Weapsy.Domain.Sites.Rules;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;
using Weapsy.Reporting.Sites;
using Weapsy.Reporting.Sites.Queries;

namespace Weapsy.Web.Api
{
    [Route("api/[controller]")]
    public class SiteController : BaseAdminController
    {
        private readonly IDispatcher _dispatcher;
        private readonly ISiteRules _siteRules;

        public SiteController(IDispatcher dispatcher,
            ISiteRules siteRules,
            IContextService contextService)
            : base(contextService)
        {
            _dispatcher = dispatcher;
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
            _dispatcher.SendAndPublish<CreateSite, Site>(model);
            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/update")]
        public IActionResult UpdateDetails([FromBody] UpdateSiteDetails model)
        {
            _dispatcher.SendAndPublish<UpdateSiteDetails, Site>(model);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _dispatcher.SendAndPublish<DeleteSite, Site>(new DeleteSite { Id = id });
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
            var model = _dispatcher.GetResultAsync<GetAdminModel, SiteAdminModel>(new GetAdminModel { Id = id });

            if (model == null)
                return NotFound();

            return Ok(model);
        }
    }
}
