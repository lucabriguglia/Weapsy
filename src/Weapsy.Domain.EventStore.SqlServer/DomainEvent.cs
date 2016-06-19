using System;

namespace Weapsy.Domain.EventStore.SqlServer
{
    public class DomainEvent
    {
        public Guid AggregateId { get; set; }
        public int SequenceNumber { get; set; }
        public string Type { get; set; }
        public string Body { get; set; }
        public DateTime TimeStamp { get; set; }
        public Guid UserId { get; set; }        
    }
}