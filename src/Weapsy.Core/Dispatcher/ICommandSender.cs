using Weapsy.Core.Domain;

namespace Weapsy.Core.Dispatcher
{
    public interface ICommandSender
    {
        void Send<TCommand, TAggregate>(TCommand command, bool publishEvents = true) 
            where TCommand : ICommand 
            where TAggregate : IAggregateRoot;
    }
}
