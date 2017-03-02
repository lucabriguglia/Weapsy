using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Weapsy.Data.Entities;
using Weapsy.Infrastructure.Domain;
using DomainEvent = Weapsy.Data.Entities.DomainEvent;

namespace Weapsy.Data
{
    public class EventStore : IEventStore
    {
        private readonly IContextFactory _contextFactory;

        public EventStore(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void SaveEvent<TAggregate>(IDomainEvent @event) where TAggregate : IAggregateRoot
        {
            using (var context = _contextFactory.Create())
            {
                var aggregate = context.DomainAggregates.FirstOrDefault(x => x.Id == @event.AggregateRootId);

                if (aggregate == null)
                {
                    context.DomainAggregates.Add(new DomainAggregate
                    {
                        Id = @event.AggregateRootId,
                        Type = typeof(TAggregate).AssemblyQualifiedName
                    });
                }

                var currentSequenceCount = context.DomainEvents.Count(x => x.DomainAggregateId == @event.AggregateRootId);

                context.DomainEvents.Add(new DomainEvent
                {
                    DomainAggregateId = @event.AggregateRootId,
                    SequenceNumber = currentSequenceCount + 1,
                    Type = @event.GetType().AssemblyQualifiedName,
                    Body = JsonConvert.SerializeObject(@event),
                    TimeStamp = @event.TimeStamp
                    //UserId = @event.UserId
                });

                context.SaveChanges();
            }
        }

        public async Task SaveEventAsync<TAggregate>(IDomainEvent @event) where TAggregate : IAggregateRoot
        {
            using (var context = _contextFactory.Create())
            {
                var aggregate = await context.DomainAggregates.FirstOrDefaultAsync(x => x.Id == @event.AggregateRootId);

                if (aggregate == null)
                {
                    context.DomainAggregates.Add(new DomainAggregate
                    {
                        Id = @event.AggregateRootId,
                        Type = typeof(TAggregate).AssemblyQualifiedName
                    });
                }

                var currentSequenceCount = await context.DomainEvents.CountAsync(x => x.DomainAggregateId == @event.AggregateRootId);

                context.DomainEvents.Add(new DomainEvent
                {
                    DomainAggregateId = @event.AggregateRootId,
                    SequenceNumber = currentSequenceCount + 1,
                    Type = @event.GetType().AssemblyQualifiedName,
                    Body = JsonConvert.SerializeObject(@event),
                    TimeStamp = @event.TimeStamp
                    //UserId = @event.UserId
                });

                await context.SaveChangesAsync();
            }
        }

        public IEnumerable<IDomainEvent> GetEvents(Guid aggregateId)
        {
            using (var context = _contextFactory.Create())
            {
                var result = new List<IDomainEvent>();

                var entities = context.DomainEvents
                    .Where(x => x.DomainAggregateId == aggregateId)
                    .OrderByDescending(x => x.SequenceNumber)
                    .ToList();

                foreach (var entity in entities)
                {
                    var @event = JsonConvert.DeserializeObject(entity.Body, Type.GetType(entity.Type));
                    result.Add((IDomainEvent)@event);
                }

                return result;
            }
        }

        public async Task<IEnumerable<IDomainEvent>> GetEventsAsync(Guid aggregateId)
        {
            using (var context = _contextFactory.Create())
            {
                var result = new List<IDomainEvent>();

                var entities = await context.DomainEvents
                    .Where(x => x.DomainAggregateId == aggregateId)
                    .OrderByDescending(x => x.SequenceNumber)
                    .ToListAsync();

                foreach (var entity in entities)
                {
                    var @event = JsonConvert.DeserializeObject(entity.Body, Type.GetType(entity.Type));
                    result.Add((IDomainEvent)@event);
                }

                return result;
            }
        }
    }
}
