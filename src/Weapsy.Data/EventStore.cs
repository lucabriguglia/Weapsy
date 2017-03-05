using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Weapsy.Data.Entities;
using Weapsy.Infrastructure.Domain;
using DomainEvent = Weapsy.Data.Entities.DomainEvent;
using Microsoft.AspNetCore.Http;

namespace Weapsy.Data
{
    public class EventStore : IEventStore
    {
        private readonly IContextFactory _contextFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EventStore(IContextFactory contextFactory, IHttpContextAccessor httpContextAccessor)
        {
            _contextFactory = contextFactory;
            _httpContextAccessor = httpContextAccessor;
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

                var userId = Guid.Empty;

                if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    userId = context.Users
                        .Where(x => x.UserName == _httpContextAccessor.HttpContext.User.Identity.Name)
                        .Select(x => x.Id)
                        .FirstOrDefault();
                }

                context.DomainEvents.Add(new DomainEvent
                {
                    DomainAggregateId = @event.AggregateRootId,
                    SequenceNumber = currentSequenceCount + 1,
                    Type = @event.GetType().AssemblyQualifiedName,
                    Body = JsonConvert.SerializeObject(@event),
                    TimeStamp = @event.TimeStamp,
                    UserId = userId
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

                var userId = Guid.Empty;

                if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    userId = await context.Users
                        .Where(x => x.UserName == _httpContextAccessor.HttpContext.User.Identity.Name)
                        .Select(x => x.Id)
                        .FirstOrDefaultAsync();
                }

                context.DomainEvents.Add(new DomainEvent
                {
                    DomainAggregateId = @event.AggregateRootId,
                    SequenceNumber = currentSequenceCount + 1,
                    Type = @event.GetType().AssemblyQualifiedName,
                    Body = JsonConvert.SerializeObject(@event),
                    TimeStamp = @event.TimeStamp,
                    UserId = userId
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
