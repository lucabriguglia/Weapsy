using System.Collections.Generic;
using FluentValidation;
using Weapsy.Domain.Users.Commands;
using Weapsy.Infrastructure.Dispatcher;

namespace Weapsy.Domain.Users.Handlers
{
    public class CreateUserHandler : ICommandHandler<CreateUser>
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<CreateUser> _validator;

        public CreateUserHandler(IUserRepository userRepository, IValidator<CreateUser> validator)
        {
            _userRepository = userRepository;
            _validator = validator;
        }

        public IEnumerable<IEvent> Handle(CreateUser command)
        {
            var user = User.CreateNew(command, _validator);

            _userRepository.Create(user);

            return user.Events;
        }
    }
}
