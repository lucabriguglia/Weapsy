using FluentValidation;
using Weapsy.Domain.Users.Commands;
using System.Threading.Tasks;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.Users.Handlers
{
    public class CreateUserHandler : ICommandHandlerWithAggregateAsync<CreateUser>
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<CreateUser> _validator;

        public CreateUserHandler(IUserRepository userRepository, IValidator<CreateUser> validator)
        {
            _userRepository = userRepository;
            _validator = validator;
        }

        public async Task<IAggregateRoot> HandleAsync(CreateUser command)
        {
            var user = User.CreateNew(command, _validator);

            await _userRepository.CreateAsync(user);

            return user;
        }
    }
}
