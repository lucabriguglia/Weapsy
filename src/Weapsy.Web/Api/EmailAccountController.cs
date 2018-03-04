using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Cqrs;
using Weapsy.Domain.EmailAccounts;
using Weapsy.Domain.EmailAccounts.Commands;
using Weapsy.Domain.EmailAccounts.Rules;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;
using Weapsy.Reporting.EmailAccounts;
using Weapsy.Reporting.EmailAccounts.Queries;

namespace Weapsy.Web.Api
{
    [Route("api/[controller]")]
    public class EmailAccountController : BaseAdminController
    {
        private readonly IDispatcher _dispatcher;
        private readonly IEmailAccountRules _emailAccountRules;

        public EmailAccountController(IDispatcher dispatcher,
            IEmailAccountRules emailAccountRules,
            IContextService contextService)
            : base(contextService)
        {
            _dispatcher = dispatcher;
            _emailAccountRules = emailAccountRules;            
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var model = await _dispatcher.GetResultAsync<GetAllEmailAccounts, IEnumerable<EmailAccountModel>>(new GetAllEmailAccounts { SiteId = SiteId });
            return Ok(model);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var model = await _dispatcher.GetResultAsync<GetEmailAccount, EmailAccountModel>(new GetEmailAccount { SiteId = SiteId, Id = id });

            if (model == null)
                return NotFound();

            return Ok(model);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateEmailAccount model)
        {
            model.SiteId = SiteId;
            model.Id = Guid.NewGuid();
            _dispatcher.SendAndPublish<CreateEmailAccount, EmailAccount>(model);
            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/update")]
        public IActionResult UpdateDetails([FromBody] UpdateEmailAccountDetails model)
        {
            model.SiteId = SiteId;
            _dispatcher.SendAndPublish<UpdateEmailAccountDetails, EmailAccount>(model);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
           _dispatcher.SendAndPublish<DeleteEmailAccount, EmailAccount>(new DeleteEmailAccount
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
