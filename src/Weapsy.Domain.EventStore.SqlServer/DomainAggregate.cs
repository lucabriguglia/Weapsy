using System;

namespace Weapsy.Domain.EventStore.SqlServer
{
    public class DomainAggregate
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
    }
}