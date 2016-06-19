using Weapsy.Mvc.Context;
using Microsoft.AspNetCore.Authorization;

namespace Weapsy.Mvc.Controllers
{
    [Authorize(Roles = "Administrator")]
    public abstract class BaseAdminController : BaseController
    {
        public BaseAdminController(IContextService contextService) : base(contextService)
        {
        }
    }
}