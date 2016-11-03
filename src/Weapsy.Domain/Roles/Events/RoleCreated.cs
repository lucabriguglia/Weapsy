using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Roles.Events
{
    public class RoleCreated : Event
    {
        public string Name { get; set; }
    }
}
