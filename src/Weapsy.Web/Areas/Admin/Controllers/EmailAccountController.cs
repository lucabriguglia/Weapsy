using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Cqrs;
using Weapsy.Domain.EmailAccounts;
using Weapsy.Domain.EmailAccounts.Commands;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;
using Weapsy.Reporting.EmailAccounts;
using Weapsy.Reporting.EmailAccounts.Queries;

namespace Weapsy.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmailAccountController : BaseAdminController
    {
        private readonly IDispatcher _dispatcher;

        public EmailAccountController(IDispatcher dispatcher,
            IContextService contextService)
            : base(contextService)
        {
            _dispatcher = dispatcher;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _dispatcher.GetResultAsync<GetAllEmailAccounts, IEnumerable<EmailAccountModel>>(new GetAllEmailAccounts { SiteId = SiteId });
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
            _dispatcher.SendAndPublish<CreateEmailAccount, EmailAccount>(model);
            return new NoContentResult();
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await _dispatcher.GetResultAsync<GetEmailAccount, EmailAccountModel>(new GetEmailAccount { SiteId = SiteId, Id = id });

            if (model == null)
                return NotFound();

            return View(model);
        }

        public IActionResult Update(UpdateEmailAccountDetails model)
        {
            model.SiteId = SiteId;
            _dispatcher.SendAndPublish<UpdateEmailAccountDetails, EmailAccount>(model);
            return new NoContentResult();
        }
    }
}
