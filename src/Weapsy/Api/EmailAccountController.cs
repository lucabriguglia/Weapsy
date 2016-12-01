using System;
using System.Threading.Tasks;
using Weapsy.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Reporting.EmailAccounts;
using Weapsy.Domain.EmailAccounts.Commands;
using Weapsy.Domain.EmailAccounts;
using Weapsy.Infrastructure.Dispatcher;
using Weapsy.Domain.EmailAccounts.Rules;
using Weapsy.Mvc.Context;

namespace Weapsy.Api
{
    [Route("api/[controller]")]
    public class EmailAccountController : BaseAdminController
    {
        private readonly IEmailAccountFacade _emailAccountFacade;
        private readonly ICommandSender _commandSender;
        private readonly IEmailAccountRules _emailAccountRules;

        public EmailAccountController(IEmailAccountFacade emailAccountFacade,
            ICommandSender commandSender,
            IEmailAccountRules emailAccountRules,
            IContextService contextService)
            : base(contextService)
        {
            _emailAccountFacade = emailAccountFacade;
            _commandSender = commandSender;
            _emailAccountRules = emailAccountRules;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var emailAccounts = _emailAccountFacade.GetAll(SiteId);
            return Ok(emailAccounts);
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var emailAccount = _emailAccountFacade.Get(SiteId, id);
            if (emailAccount == null)
                return NotFound();
            return Ok(emailAccount);
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
