using System.Threading.Tasks;
using Weapsy.Core.Domain;
using Weapsy.Domain.Model.Roles.Events;

namespace Weapsy.Domain.Model.Users.Handlers
{
    public class RoleCreatedHandler : IEventHandler<RoleCreated>
    {
        public RoleCreatedHandler()
        {
        }

        public Task Handle(RoleCreated @event)
        {
            return null;
        }
    }
}
