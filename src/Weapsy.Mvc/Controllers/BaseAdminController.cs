using Weapsy.Mvc.Context;
using Microsoft.AspNetCore.Authorization;

namespace Weapsy.Mvc.Controllers
{
    [Authorize(Roles = "Administrator")]
    public abstract class BaseAdminController : BaseController
    {
        protected BaseAdminController(IContextService contextService) 
            : base(contextService)
        {
        }
    }
}