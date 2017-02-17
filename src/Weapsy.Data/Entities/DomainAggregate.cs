using System;
using System.Collections.Generic;

namespace Weapsy.Data.Entities
{
    public class DomainAggregate
    {
        public Guid Id { get; set; }
        public string Type { get; set; }

        public virtual ICollection<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    }
}