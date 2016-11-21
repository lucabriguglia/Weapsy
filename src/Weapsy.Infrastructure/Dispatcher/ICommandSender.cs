using System.Threading.Tasks;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Infrastructure.Dispatcher
{
    public interface ICommandSender
    {
        void Send<TCommand, TAggregate>(TCommand command, bool publishEvents = true) 
            where TCommand : ICommand 
            where TAggregate : IAggregateRoot;

        Task SendAsync<TCommand, TAggregate>(TCommand command, bool publishEvents = true)
            where TCommand : ICommand
            where TAggregate : IAggregateRoot;
    }
}
