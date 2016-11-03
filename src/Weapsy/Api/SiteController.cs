using System;
using System.Threading.Tasks;
using Weapsy.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Reporting.Sites;
using Weapsy.Infrastructure.Dispatcher;
using Weapsy.Domain.Sites.Rules;
using Weapsy.Domain.Sites.Commands;
using Weapsy.Domain.Sites;
using Weapsy.Mvc.Context;

namespace Weapsy.Api
{
    [Route("api/[controller]")]
    public class SiteController : BaseAdminController
    {
        private readonly ICommandSender _commandSender;
        private readonly ISiteFacade _siteFacade;        
        private readonly ISiteRules _siteRules;

        public SiteController(ICommandSender commandSender,
            ISiteFacade siteFacade,
            ISiteRules siteRules,
            IContextService contextService)
            : base(contextService)
        {
            _siteFacade = siteFacade;
            _commandSender = commandSender;
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
        public async Task<IActionResult> Post([FromBody] CreateSite model)
        {
            model.Id = Guid.NewGuid();
            await Task.Run(() => _commandSender.Send<CreateSite, Site>(model));
            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/update")]
        public async Task<IActionResult> UpdateDetails([FromBody] UpdateSiteDetails model)
        {
            await Task.Run(() => _commandSender.Send<UpdateSiteDetails, Site>(model));
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await Task.Run(() => _commandSender.Send<DeleteSite, Site>(new DeleteSite { Id = id }));
            return new NoContentResult();
        }

        [HttpGet("{name}")]
        [Route("isSiteNameUnique")]
        public async Task<IActionResult> IsSiteNameUnique(string name)
        {
            var isSiteNameUnique = await Task.Run(() => _siteRules.IsSiteNameUnique(name));
            return Ok(isSiteNameUnique);
        }

        [HttpGet]
        [Route("{id}/admin-edit")]
        public async Task<IActionResult> AdminEdit(Guid id)
        {
            var model = await Task.Run(() => _siteFacade.GetAdminModel(id));

            if (model == null)
                return NotFound();

            return Ok(model);
        }
    }
}
