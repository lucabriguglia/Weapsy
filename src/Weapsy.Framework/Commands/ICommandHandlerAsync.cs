using System.Collections.Generic;
using System.Threading.Tasks;
using Weapsy.Framework.Events;

namespace Weapsy.Framework.Commands
{
    public interface ICommandHandlerAsync<in TCommand> where TCommand : ICommand
    {
        Task<IEnumerable<IEvent>> HandleAsync(TCommand command);
    }
}
