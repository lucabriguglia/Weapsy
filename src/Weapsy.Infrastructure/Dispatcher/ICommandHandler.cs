using System.Collections.Generic;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Infrastructure.Dispatcher
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        IEnumerable<IDomainEvent> Handle(TCommand command);
    }
}