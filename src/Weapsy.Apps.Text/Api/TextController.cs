using System;
using System.Threading.Tasks;
using Weapsy.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Mvc.Context;
using Weapsy.Apps.Text.Reporting;
using Weapsy.Apps.Text.Domain.Commands;
using Weapsy.Apps.Text.Domain;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Queries;

namespace Weapsy.Apps.Text.Api
{
    [Route("api/apps/text/[controller]")]
    public class TextController : BaseAdminController
    {
        private readonly ICommandSender _commandSender;
        private readonly IQueryDispatcher _queryDispatcher;

        public TextController(ICommandSender commandSender, 
            IQueryDispatcher queryDispatcher,
            IContextService contextService)
            : base(contextService)
        {
            _commandSender = commandSender;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var content = await _queryDispatcher.DispatchAsync<GetContent, string>(new GetContent {ModuleId = id});

            if (content == null)
                return NotFound();

            return Ok(content);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateTextModule model)
        {
            model.SiteId = SiteId;
            model.Id = Guid.NewGuid();
            _commandSender.Send<CreateTextModule, TextModule>(model);
            return Ok(string.Empty);
        }

        [HttpPut]
        public IActionResult Put([FromBody] AddVersion model)
        {
            model.SiteId = SiteId;
            model.VersionId = Guid.NewGuid();
            _commandSender.Send<AddVersion, TextModule>(model);
            return Ok(string.Empty);
        }
    }
}
