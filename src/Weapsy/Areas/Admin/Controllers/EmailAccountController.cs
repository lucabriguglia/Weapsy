using System;
using Weapsy.Mvc.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Reporting.EmailAccounts;
using Weapsy.Domain.EmailAccounts;
using Weapsy.Domain.EmailAccounts.Commands;
using Weapsy.Infrastructure.Dispatcher;
using Weapsy.Mvc.Context;

namespace Weapsy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmailAccountController : BaseAdminController
    {
        private readonly IEmailAccountFacade _emailAccountFacade;
        private readonly ICommandSender _commandSender;

        public EmailAccountController(IEmailAccountFacade emailAccountFacade,
            ICommandSender commandSender,
            IContextService contextService)
            : base(contextService)
        {
            _emailAccountFacade = emailAccountFacade;
            _commandSender = commandSender;
        }

        public IActionResult Index()
        {
            var model = _emailAccountFacade.GetAll(SiteId);
            return View(model);
        }

        public IActionResult Create()
        {
            var model = new EmailAccountModel();
            return View(model);
        }

        public async Task<IActionResult> Save(CreateEmailAccount model)
        {
            model.SiteId = SiteId;
            model.Id = Guid.NewGuid();
            await Task.Run(() => _commandSender.Send<CreateEmailAccount, EmailAccount>(model));
            return new NoContentResult();
        }

        public IActionResult Edit(Guid id)
        {
            var model = _emailAccountFacade.Get(SiteId, id);

            if (model == null)
                return NotFound();

            return View(model);
        }

        public async Task<IActionResult> Update(UpdateEmailAccountDetails model)
        {
            model.SiteId = SiteId;
            await Task.Run(() => _commandSender.Send<UpdateEmailAccountDetails, EmailAccount>(model));
            return new NoContentResult();
        }
    }
}
