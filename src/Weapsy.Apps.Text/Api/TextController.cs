using System;
using System.Threading.Tasks;
using Weapsy.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Mvc.Context;
using Weapsy.Apps.Text.Reporting;
using Weapsy.Apps.Text.Domain.Commands;
using Weapsy.Apps.Text.Domain;
using Weapsy.Cqrs;

namespace Weapsy.Apps.Text.Api
{
    [Route("api/apps/text/[controller]")]
    public class TextController : BaseAdminController
    {
        private readonly IDispatcher _dispatcher;

        public TextController(IDispatcher commandSender, 
            IContextService contextService)
            : base(contextService)
        {
            _dispatcher = commandSender;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var content = await _dispatcher.GetResultAsync<GetContent, string>(new GetContent {ModuleId = id});

            if (content == null)
                return NotFound();

            return Ok(content);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateTextModule model)
        {
            model.SiteId = SiteId;
            model.Id = Guid.NewGuid();
            _dispatcher.SendAndPublish<CreateTextModule, TextModule>(model);
            return Ok(string.Empty);
        }

        [HttpPut]
        public IActionResult Put([FromBody] AddVersion model)
        {
            model.SiteId = SiteId;
            model.VersionId = Guid.NewGuid();
            _dispatcher.SendAndPublish<AddVersion, TextModule>(model);
            return Ok(string.Empty);
        }
    }
}
