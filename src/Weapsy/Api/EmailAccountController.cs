using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Weapsy.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Reporting.EmailAccounts;
using Weapsy.Domain.EmailAccounts.Commands;
using Weapsy.Domain.EmailAccounts;
using Weapsy.Domain.EmailAccounts.Rules;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Queries;
using Weapsy.Mvc.Context;
using Weapsy.Reporting.EmailAccounts.Queries;

namespace Weapsy.Api
{
    [Route("api/[controller]")]
    public class EmailAccountController : BaseAdminController
    {
        private readonly ICommandSender _commandSender;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly IEmailAccountRules _emailAccountRules;

        public EmailAccountController(ICommandSender commandSender,
            IQueryDispatcher queryDispatcher,
            IEmailAccountRules emailAccountRules,
            IContextService contextService)
            : base(contextService)
        {
            _commandSender = commandSender;
            _queryDispatcher = queryDispatcher;
            _emailAccountRules = emailAccountRules;            
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var model = await _queryDispatcher.DispatchAsync<GetAllEmailAccounts, IEnumerable<EmailAccountModel>>(new GetAllEmailAccounts { SiteId = SiteId });
            return Ok(model);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var model = await _queryDispatcher.DispatchAsync<GetEmailAccount, EmailAccountModel>(new GetEmailAccount { SiteId = SiteId, Id = id });

            if (model == null)
                return NotFound();

            return Ok(model);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateEmailAccount model)
        {
            model.SiteId = SiteId;
            model.Id = Guid.NewGuid();
            _commandSender.Send<CreateEmailAccount, EmailAccount>(model);
            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/update")]
        public IActionResult UpdateDetails([FromBody] UpdateEmailAccountDetails model)
        {
            model.SiteId = SiteId;
            _commandSender.Send<UpdateEmailAccountDetails, EmailAccount>(model);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
           _commandSender.Send<DeleteEmailAccount, EmailAccount>(new DeleteEmailAccount
            {
                SiteId = SiteId,
                Id = id
            });
            return new NoContentResult();
        }

        [HttpGet("{name}")]
        [Route("IsEmailAccountAddressUnique")]
        public IActionResult IsEmailAccountAddressUnique(string email)
        {
            var isEmailAccountAddressUnique = _emailAccountRules.IsEmailAccountAddressUnique(SiteId, email);
            return Ok(isEmailAccountAddressUnique);
        }
    }
}
