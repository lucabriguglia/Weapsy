using System.Collections.Generic;
using FluentValidation;
using Weapsy.Domain.Users.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;
using System.Threading.Tasks;

namespace Weapsy.Domain.Users.Handlers
{
    public class CreateUserHandler : ICommandHandlerAsync<CreateUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<CreateUserCommand> _validator;

        public CreateUserHandler(IUserRepository userRepository, IValidator<CreateUserCommand> validator)
        {
            _userRepository = userRepository;
            _validator = validator;
        }

        public async Task<IEnumerable<IEvent>> HandleAsync(CreateUserCommand command)
        {
            var user = User.CreateNew(command, _validator);

            await _userRepository.CreateAsync(user);

            return user.Events;
        }
    }
}
