using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Roles.Events
{
    public class RoleDeleted : DomainEvent
    {
        public string Name { get; set; }
    }
}
