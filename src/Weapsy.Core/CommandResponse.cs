using System.Collections.Generic;
using System.Linq;

namespace Weapsy.Core
{
    public class CommandResponse
    {
        public IList<IEvent> Events { get; set; }

        public CommandResponse(params IEvent[] events)
        {
            Events = events.ToList();
        }
    }
}