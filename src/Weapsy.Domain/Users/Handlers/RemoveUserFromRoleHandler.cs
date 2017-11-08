using System.Collections.Generic;
using Weapsy.Domain.Users.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;
using System.Threading.Tasks;
using Weapsy.Domain.Users.Events;

namespace Weapsy.Domain.Users.Handlers
{
    public class RemoveUserFromRoleHandler : ICommandHandlerAsync<RemoveUserFromRoleCommand>
    {
        private readonly IUserRepository _userRepository;

        public RemoveUserFromRoleHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<IEvent>> HandleAsync(RemoveUserFromRoleCommand command)
        {
            await _userRepository.RemoveFromRoleAsync(command.Id, command.RoleName);

            return new List<IEvent>
            {
                new UserRemovedFromRoleEvent
                {
                    AggregateRootId = command.Id,
                    RoleName = command.RoleName
                }
            };
        }
    }
}
