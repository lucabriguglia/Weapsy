using System.Collections.Generic;
using Weapsy.Domain.Users.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;
using System.Threading.Tasks;
using Weapsy.Domain.Users.Events;

namespace Weapsy.Domain.Users.Handlers
{
    public class AddUserToRoleHandler : ICommandHandlerAsync<AddUserToRoleCommand>
    {
        private readonly IUserRepository _userRepository;

        public AddUserToRoleHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<IEvent>> HandleAsync(AddUserToRoleCommand command)
        {
            await _userRepository.AddToRoleAsync(command.Id, command.RoleName);

            return new List<IEvent>
            {
                new UserAddedToRoleEvent
                {
                    AggregateRootId = command.Id,
                    RoleName = command.RoleName
                }
            };
        }
    }
}
