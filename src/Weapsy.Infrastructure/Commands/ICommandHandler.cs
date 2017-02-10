using System.Collections.Generic;
using Weapsy.Infrastructure.Events;

namespace Weapsy.Infrastructure.Commands
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        IEnumerable<IEvent> Handle(TCommand command);
    }
}