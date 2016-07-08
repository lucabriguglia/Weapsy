using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Roles.Events
{
    public class RoleCreated : Event
    {
        public string Name { get; set; }
    }
}
