using System;
using System.Collections.Generic;

namespace Weapsy.Framework.Domain
{
    public interface IAggregateRoot
    {
        Guid Id { get; }
        ICollection<IDomainEvent> Events { get; }
    }
}
