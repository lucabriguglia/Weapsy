using System.Collections.Generic;
using System.Threading.Tasks;
using Weapsy.Infrastructure.Events;

namespace Weapsy.Infrastructure.Commands
{
    public interface ICommandHandlerAsync<in TCommand> where TCommand : ICommand
    {
        Task<IEnumerable<IEvent>> HandleAsync(TCommand command);
    }
}
