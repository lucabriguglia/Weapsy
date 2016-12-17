using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Roles.Events
{
    public class RoleCreated : DomainEvent
    {
        public string Name { get; set; }
    }
}
