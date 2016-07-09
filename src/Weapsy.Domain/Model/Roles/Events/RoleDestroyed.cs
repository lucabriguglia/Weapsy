using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Roles.Events
{
    public class RoleDestroyed : Event
    {
        public string Name { get; set; }
    }
}
