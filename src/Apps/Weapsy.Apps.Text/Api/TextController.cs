using System;
using System.Threading.Tasks;
using Weapsy.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Infrastructure.Dispatcher;
using Weapsy.Mvc.Context;
using Weapsy.Apps.Text.Reporting;
using Weapsy.Apps.Text.Domain.Commands;
using Weapsy.Apps.Text.Domain;

namespace Weapsy.Apps.Text.Api
{
    [Route("api/apps/text/[controller]")]
    public class TextController : BaseAdminController
    {
        private readonly ITextModuleFacade _textFacade;
        private readonly ICommandSender _commandSender;

        public TextController(ITextModuleFacade textFacade,
            ICommandSender commandSender,
            IContextService contextService)
            : base(contextService)
        {
            _textFacade = textFacade;
            _commandSender = commandSender;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var text = await Task.Run(() => _textFacade.GetContent(id));
            if (text == null) return NotFound();
            return Ok(text);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateTextModule model)
        {
            model.SiteId = SiteId;
            model.Id = Guid.NewGuid();
            await Task.Run(() => _commandSender.Send<CreateTextModule, TextModule>(model));
            return Ok(string.Empty);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] AddVersion model)
        {
            model.SiteId = SiteId;
            model.VersionId = Guid.NewGuid();
            await Task.Run(() => _commandSender.Send<AddVersion, TextModule>(model));
            return Ok(string.Empty);
        }
    }
}
