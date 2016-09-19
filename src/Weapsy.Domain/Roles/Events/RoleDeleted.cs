using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Roles.Events
{
    public class RoleDeleted : Event
    {
        public string Name { get; set; }
    }
}
