using System;
using System.Collections.Generic;

namespace Weapsy.Infrastructure.Domain
{
    public abstract class AggregateRoot : IAggregateRoot
    {
        public Guid Id { get; protected set; }
        public ICollection<IEvent> Events { get; protected set; } = new List<IEvent>();

        protected AggregateRoot()
        {
            Id = Guid.Empty;
        }

        protected AggregateRoot(Guid id)
        {
            if (id == Guid.Empty)
                id = new Guid();

            Id = id;
        }

        protected void AddEvent(IEvent @event)
        {
            Events.Add(@event);
        }
    }
}
