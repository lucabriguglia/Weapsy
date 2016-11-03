using System;

namespace Weapsy.Infrastructure.Domain
{
    public interface IEntity
    {
        Guid Id { get; }
    }
}
