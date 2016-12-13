using System.Collections.Generic;
using Weapsy.Infrastructure.Dispatcher;

namespace Weapsy.Infrastructure.Domain
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        IEnumerable<IDomainEvent> Handle(TCommand command);
    }
}