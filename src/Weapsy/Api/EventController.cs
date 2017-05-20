using Microsoft.AspNetCore.Mvc;
using Weapsy.Framework.Domain;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;

namespace Weapsy.Api
{
    [Route("api/[controller]")]
    public class EventController : BaseAdminController
    {
        private readonly IEventStore _eventStore;

        public EventController(IEventStore eventStore,
            IContextService contextService)
            : base(contextService)
        {
            _eventStore = eventStore;
        }
    }
}
