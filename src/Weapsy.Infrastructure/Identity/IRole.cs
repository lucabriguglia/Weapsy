using System;

namespace Weapsy.Infrastructure.Identity
{
    public interface IRole
    {
        Guid Id { get; }
        string Name { get; }
    }
}
