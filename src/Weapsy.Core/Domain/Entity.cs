using System;

namespace Weapsy.Core.Domain
{
    public abstract class Entity : IEntity
    {
        protected Entity()
        {
            Id = Guid.Empty;
        }

        protected Entity(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; protected set; }
    }
}
