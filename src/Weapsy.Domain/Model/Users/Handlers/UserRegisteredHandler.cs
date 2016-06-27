using System.Threading.Tasks;
using Weapsy.Core.Dispatcher;
using Weapsy.Core.Domain;
using Weapsy.Domain.Model.Users.Commands;
using Weapsy.Domain.Model.Users.Events;

namespace Weapsy.Domain.Model.Users.Handlers
{
    public class UserRegisteredHandler : IEventHandler<UserRegistered>
    {
        private readonly ICommandSender _commandSender;

        public UserRegisteredHandler(ICommandSender commandSender)
        {
            _commandSender = commandSender;
        }

        public async Task Handle(UserRegistered @event)
        {
            await Task.Run(() => _commandSender.Send<CreateUser, User>(new CreateUser
            {
                Id = @event.AggregateRootId,
                Email = @event.Email,
                UserName = @event.UserName
            }));
        }
    }
}
