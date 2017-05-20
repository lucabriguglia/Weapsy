using System.Collections.Generic;
using Weapsy.Framework.Events;

namespace Weapsy.Framework.Commands
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        IEnumerable<IEvent> Handle(TCommand command);
    }
}