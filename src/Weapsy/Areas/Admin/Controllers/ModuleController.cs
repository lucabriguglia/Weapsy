using Weapsy.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Reporting.Modules;
using Weapsy.Infrastructure.Dispatcher;
using Weapsy.Mvc.Context;

namespace Weapsy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ModuleController : BaseAdminController
    {
        private readonly IModuleFacade _moduleFacade;
        private readonly ICommandSender _commandSender;

        public ModuleController(IModuleFacade moduleFacade,
            ICommandSender commandSender,
            IContextService contextService)
            : base(contextService)
        {
            _moduleFacade = moduleFacade;
            _commandSender = commandSender;
        }
    }
}
