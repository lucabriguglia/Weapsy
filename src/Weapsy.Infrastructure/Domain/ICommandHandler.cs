using System.Collections.Generic;

namespace Weapsy.Infrastructure.Domain
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        IEnumerable<IEvent> Handle(TCommand command);
    }
}