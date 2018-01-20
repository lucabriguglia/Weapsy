using Microsoft.AspNetCore.Mvc;
using Weapsy.Framework.Commands;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;

namespace Weapsy.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ModuleController : BaseAdminController
    {
        private readonly ICommandSender _commandSender;

        public ModuleController(ICommandSender commandSender,
            IContextService contextService)
            : base(contextService)
        {
            _commandSender = commandSender;
        }
    }
}
