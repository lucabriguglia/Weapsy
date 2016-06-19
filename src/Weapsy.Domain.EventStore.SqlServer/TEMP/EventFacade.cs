//using Microsoft.Data.Entity;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Weapsy.Data;
//using Weapsy.Data.Entities;
//using Weapsy.Reporting.Data.Mappers;
//using Weapsy.Reporting.Events;

//namespace Weapsy.Reporting.Data.Facades
//{
//    public class EventFacade : IEventFacade
//    {
//        private readonly DbSet<DomainEvent> _domainEvents;

//        public EventFacade(WeapsyDbContext context)
//        {
//            _domainEvents = context.Set<DomainEvent>();
//        }

//        public async Task<IEnumerable<EventDto>> GetAllByAggregateIdAsync(Guid aggregateId)
//        {
//            var entities = await _domainEvents
//                .Where(x => x.AggregateId == aggregateId)
//                .OrderByDescending(x => x.SequenceNumber)
//                .ToListAsync();

//            return entities.ToDtos();
//        }
//    }
//}
