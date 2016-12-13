using System.Collections.Generic;

namespace Weapsy.Infrastructure.Dispatcher
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        IEnumerable<IEvent> Handle(TCommand command);
    }
}