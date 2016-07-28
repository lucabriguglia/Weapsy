using System;
using System.Threading.Tasks;
using Weapsy.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Reporting.EmailAccounts;
using Weapsy.Domain.Model.EmailAccounts.Commands;
using Weapsy.Domain.Model.EmailAccounts;
using Weapsy.Core.Dispatcher;
using Weapsy.Domain.Model.EmailAccounts.Rules;
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
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateEmailAccount model)
        {
            model.SiteId = SiteId;
            model.Id = Guid.NewGuid();
            await Task.Run(() => _commandSender.Send<CreateEmailAccount, EmailAccount>(model));
            return Ok(string.Empty);
        }

        [HttpPut]
        [Route("{id}/update")]
        public async Task<IActionResult> UpdateDetails([FromBody] UpdateEmailAccountDetails model)
        {
            model.SiteId = SiteId;
            await Task.Run(() => _commandSender.Send<UpdateEmailAccountDetails, EmailAccount>(model));
            return Ok(string.Empty);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await Task.Run(() => _commandSender.Send<DeleteEmailAccount, EmailAccount>(new DeleteEmailAccount
            {
                SiteId = SiteId,
                Id = id
            }));
            return Ok(string.Empty);
        }

        [HttpGet("{name}")]
        [Route("IsEmailAccountAddressUnique")]
        public IActionResult IsEmailAccountNameUnique(string email)
        {
            var isEmailAccountAddressUnique = _emailAccountRules.IsEmailAccountAddressUnique(SiteId, email);
            return Ok(isEmailAccountAddressUnique);
        }

        [HttpGet]
        [Route("{id}/admin-list")]
        public IActionResult AdminList()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{id}/admin-edit")]
        public IActionResult AdminEdit(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
