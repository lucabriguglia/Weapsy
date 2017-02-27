using System.Collections.Generic;
using Weapsy.Domain.Users.Commands;
using Weapsy.Infrastructure.Commands;
using Weapsy.Infrastructure.Events;
using System.Threading.Tasks;
using Weapsy.Domain.Users.Events;

namespace Weapsy.Domain.Users.Handlers
{
    public class RemoveUserFromRoleHandler : ICommandHandlerAsync<RemoveUserFromRole>
    {
        private readonly IUserRepository _userRepository;

        public RemoveUserFromRoleHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<IEvent>> HandleAsync(RemoveUserFromRole command)
        {
            await _userRepository.RemoveFromRoleAsync(command.Id, command.RoleName);

            return new List<IEvent>
            {
                new UserRemovedFromRole
                {
                    AggregateRootId = command.Id,
                    RoleName = command.RoleName
                }
            };
        }
    }
}
