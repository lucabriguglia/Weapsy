using Microsoft.AspNetCore.Mvc;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;

namespace Weapsy.Web.Api
{
    [Route("api/[controller]")]
    public class EventController : BaseAdminController
    {
        public EventController(IContextService contextService)
            : base(contextService)
        {
        }
    }
}
