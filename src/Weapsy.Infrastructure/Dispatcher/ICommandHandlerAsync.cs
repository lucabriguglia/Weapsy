using System.Collections.Generic;
using System.Threading.Tasks;

namespace Weapsy.Infrastructure.Dispatcher
{
    public interface ICommandHandlerAsync<in TCommand> where TCommand : ICommand
    {
        Task<IEnumerable<IEvent>> HandleAsync(TCommand command);
    }
}
