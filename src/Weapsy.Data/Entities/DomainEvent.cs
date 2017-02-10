using System;

namespace Weapsy.Data.Entities
{
    public class DomainEvent
    {
        public string DomainAggregateId { get; set; }
        public int SequenceNumber { get; set; }
        public string Type { get; set; }
        public string Body { get; set; }
        public DateTime TimeStamp { get; set; }
        public string UserId { get; set; }

        public virtual DomainAggregate DomainAggregate { get; set; }
    }
}