using System;
using System.Collections.Generic;
using Weapsy.Mvc.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Reporting.EmailAccounts;
using Weapsy.Domain.EmailAccounts;
using Weapsy.Domain.EmailAccounts.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Queries;
using Weapsy.Mvc.Context;
using Weapsy.Reporting.EmailAccounts.Queries;

namespace Weapsy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmailAccountController : BaseAdminController
    {
        private readonly ICommandSender _commandSender;
        private readonly IQueryDispatcher _queryDispatcher;

        public EmailAccountController(ICommandSender commandSender,
            IQueryDispatcher queryDispatcher,
            IContextService contextService)
            : base(contextService)
        {
            _commandSender = commandSender;
            _queryDispatcher = queryDispatcher;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _queryDispatcher.DispatchAsync<GetAllEmailAccounts, IEnumerable<EmailAccountModel>>(new GetAllEmailAccounts { SiteId = SiteId });
            return View(model);
        }

        public IActionResult Create()
        {
            var model = new EmailAccountModel();
            return View(model);
        }

        public IActionResult Save(CreateEmailAccount model)
        {
            model.SiteId = SiteId;
            model.Id = Guid.NewGuid();
            _commandSender.Send<CreateEmailAccount, EmailAccount>(model);
            return new NoContentResult();
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await _queryDispatcher.DispatchAsync<GetEmailAccount, EmailAccountModel>(new GetEmailAccount { SiteId = SiteId, Id = id });

            if (model == null)
                return NotFound();

            return View(model);
        }

        public IActionResult Update(UpdateEmailAccountDetails model)
        {
            model.SiteId = SiteId;
            _commandSender.Send<UpdateEmailAccountDetails, EmailAccount>(model);
            return new NoContentResult();
        }
    }
}
