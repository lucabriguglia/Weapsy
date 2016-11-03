using System.Collections.Generic;

namespace Weapsy.Infrastructure.Domain
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        ICollection<IEvent> Handle(TCommand command);
    }
}