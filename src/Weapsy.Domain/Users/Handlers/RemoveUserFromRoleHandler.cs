using Weapsy.Domain.Users.Commands;
using System.Threading.Tasks;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.Users.Handlers
{
    public class RemoveUserFromRoleHandler : ICommandHandlerWithAggregateAsync<RemoveUserFromRole>
    {
        private readonly IUserRepository _userRepository;

        public RemoveUserFromRoleHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IAggregateRoot> HandleAsync(RemoveUserFromRole command)
        {
            await _userRepository.RemoveFromRoleAsync(command.Id, command.RoleName);

            return new User();
        }
    }
}
