using FluentValidation;
using System.Threading.Tasks;
using Weapsy.Domain.Users.Commands;
using Weapsy.Domain.Users.Events;
using Weapsy.Infrastructure.Dispatcher;

namespace Weapsy.Domain.Users.Handlers
{
    public class UserRegisteredHandler : IEventHandlerAsync<UserRegistered>
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<CreateUser> _validator;

        public UserRegisteredHandler(IUserRepository userRepository, IValidator<CreateUser> validator)
        {
            _userRepository = userRepository;
            _validator = validator;
        }

        public Task HandleAsync(UserRegistered @event)
        {
            return Task.Run(() =>
            {
                var command = new CreateUser
                {
                    Id = @event.AggregateRootId,
                    Email = @event.Email,
                    UserName = @event.UserName
                };

                var user = User.CreateNew(command, _validator);

                _userRepository.Create(user);                
            });
        }
    }
}
