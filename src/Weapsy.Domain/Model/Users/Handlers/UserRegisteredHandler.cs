using System;
using System.Threading.Tasks;
using Weapsy.Core.Domain;
using Weapsy.Domain.Model.Users;
using Weapsy.Domain.Model.Users.Events;

namespace Weapsy.Domain.Model.User.Handlers
{
    public class UserRegisteredHandler : IEventHandler<UserRegistered>
    {
        private readonly IUserRepository _userRepository;

        public UserRegisteredHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task Handle(UserRegistered @event)
        {
            throw new NotImplementedException();
        }
    }
}
