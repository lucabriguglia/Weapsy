using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.EventStore.SqlServer
{
    public class SqlServerEventStore : IEventStore
    {
        private readonly EventStoreDbContext _context;
        private readonly DbSet<DomainAggregate> _aggregates;
        private readonly DbSet<DomainEvent> _events;

        public SqlServerEventStore(EventStoreDbContext context)
        {
            _context = context;
            _aggregates = context.Set<DomainAggregate>();
            _events = context.Set<DomainEvent>();
        }

        public void SaveEvent<TAggregate>(IDomainEvent @event) where TAggregate : IAggregateRoot
        {
            var aggregate = _aggregates.FirstOrDefault(x => x.Id == @event.AggregateRootId);

            if (aggregate == null)
            {
                _aggregates.Add(new DomainAggregate
                {
                    Id = @event.AggregateRootId,
                    Type = typeof(TAggregate).AssemblyQualifiedName
                });
            }

            var currentSequenceCount = _events.Count(x => x.AggregateId == @event.AggregateRootId);

            _events.Add(new DomainEvent
            {
                AggregateId = @event.AggregateRootId,
                SequenceNumber = currentSequenceCount + 1,
                Type = @event.GetType().AssemblyQualifiedName,
                Body = JsonConvert.SerializeObject(@event),
                TimeStamp = @event.TimeStamp
                //UserId = @event.UserId
            });

            _context.SaveChanges();     
        }

        public async Task SaveEventAsync<TAggregate>(IDomainEvent @event) where TAggregate : IAggregateRoot
        {
            var aggregate = await _aggregates.FirstOrDefaultAsync(x => x.Id == @event.AggregateRootId);

            if (aggregate == null)
            {
                _aggregates.Add(new DomainAggregate
                {
                    Id = @event.AggregateRootId,
                    Type = typeof(TAggregate).AssemblyQualifiedName
                });
            }

            var currentSequenceCount = await _events.CountAsync(x => x.AggregateId == @event.AggregateRootId);

            _events.Add(new DomainEvent
            {
                AggregateId = @event.AggregateRootId,
                SequenceNumber = currentSequenceCount + 1,
                Type = @event.GetType().AssemblyQualifiedName,
                Body = JsonConvert.SerializeObject(@event),
                TimeStamp = @event.TimeStamp
                //UserId = @event.UserId
            });

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<IDomainEvent>> GetEvents(Guid aggregateId)
        {
            var result = new List<IDomainEvent>();

            var entities = await _events
                .Where(x => x.AggregateId == aggregateId)
                .OrderByDescending(x => x.SequenceNumber)
                .ToListAsync();

            foreach (var entity in entities)
            {
                var @event = JsonConvert.DeserializeObject(entity.Body, Type.GetType(entity.Type));
                result.Add((IDomainEvent)@event);
            }

            return result;
        }

        public Task<IEnumerable<IDomainEvent>> GetEventsAsync(Guid aggregateId)
        {
            throw new NotImplementedException();
        }
    }
}
