using Weapsy.Domain.Users.Commands;
using System.Threading.Tasks;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.Users.Handlers
{
    public class AddUserToRoleHandler : ICommandHandlerWithAggregateAsync<AddUserToRole>
    {
        private readonly IUserRepository _userRepository;

        public AddUserToRoleHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IAggregateRoot> HandleAsync(AddUserToRole command)
        {
            await _userRepository.AddToRoleAsync(command.Id, command.RoleName);

            return new User();
        }
    }
}
