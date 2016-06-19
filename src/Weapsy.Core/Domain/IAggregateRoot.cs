using System;
using System.Collections.Generic;

namespace Weapsy.Core.Domain
{
    public interface IAggregateRoot
    {
        Guid Id { get; }
        ICollection<IEvent> Events { get; }
    }
}
