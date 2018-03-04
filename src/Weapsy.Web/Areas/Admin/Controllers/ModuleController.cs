using Microsoft.AspNetCore.Mvc;
using Weapsy.Cqrs;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;

namespace Weapsy.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ModuleController : BaseAdminController
    {
        private readonly IDispatcher _dispatcher;

        public ModuleController(IDispatcher dispatcher,
            IContextService contextService)
            : base(contextService)
        {
            _dispatcher = dispatcher;
        }
    }
}
