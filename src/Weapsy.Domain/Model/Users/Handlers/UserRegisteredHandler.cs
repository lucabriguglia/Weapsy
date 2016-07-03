using FluentValidation;
using System.Threading.Tasks;
using Weapsy.Core.Domain;
using Weapsy.Domain.Model.Users.Commands;
using Weapsy.Domain.Model.Users.Events;

namespace Weapsy.Domain.Model.Users.Handlers
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

        public Task Handle(UserRegistered @event)
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
