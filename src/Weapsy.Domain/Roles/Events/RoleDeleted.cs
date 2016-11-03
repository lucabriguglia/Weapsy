using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Roles.Events
{
    public class RoleDeleted : Event
    {
        public string Name { get; set; }
    }
}
