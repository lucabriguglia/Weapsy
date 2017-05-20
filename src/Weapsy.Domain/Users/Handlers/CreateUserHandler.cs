using System.Collections.Generic;
using FluentValidation;
using Weapsy.Domain.Users.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;
using System.Threading.Tasks;

namespace Weapsy.Domain.Users.Handlers
{
    public class CreateUserHandler : ICommandHandlerAsync<CreateUser>
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<CreateUser> _validator;

        public CreateUserHandler(IUserRepository userRepository, IValidator<CreateUser> validator)
        {
            _userRepository = userRepository;
            _validator = validator;
        }

        public async Task<IEnumerable<IEvent>> HandleAsync(CreateUser command)
        {
            var user = User.CreateNew(command, _validator);

            await _userRepository.CreateAsync(user);

            return user.Events;
        }
    }
}
