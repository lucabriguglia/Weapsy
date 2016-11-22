using FluentValidation;
using System.Threading.Tasks;
using Weapsy.Infrastructure.Domain;
using Weapsy.Domain.Users.Commands;
using Weapsy.Domain.Users.Events;

namespace Weapsy.Domain.Users.Handlers
{
    public class UserRegisteredHandler : IEventHandler<UserRegistered>
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<CreateUser> _validator;

        public UserRegisteredHandler(IUserRepository userRepository, IValidator<CreateUser> validator)
        {
            _userRepository = userRepository;
            _validator = validator;
        }

        public void Handle(UserRegistered @event)
        {
            var command = new CreateUser
            {
                Id = @event.AggregateRootId,
                Email = @event.Email,
                UserName = @event.UserName
            };

            var user = User.CreateNew(command, _validator);

            _userRepository.Create(user);
        }
    }
}
