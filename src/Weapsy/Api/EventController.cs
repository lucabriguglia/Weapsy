using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Weapsy.Infrastructure.Domain;
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

        [HttpGet("{aggregateId}")]
        public async Task<IActionResult> Get(Guid aggregateId)
        {
            var events = await _eventStore.GetEventsAsync(aggregateId);
            return Ok(events);
        }
    }
}
