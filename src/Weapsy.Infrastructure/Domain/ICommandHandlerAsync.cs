using System.Collections.Generic;
using System.Threading.Tasks;
using Weapsy.Infrastructure.Dispatcher;

namespace Weapsy.Infrastructure.Domain
{
    public interface ICommandHandlerAsync<in TCommand> where TCommand : ICommand
    {
        Task<IEnumerable<IEvent>> HandleAsync(TCommand command);
    }
}
