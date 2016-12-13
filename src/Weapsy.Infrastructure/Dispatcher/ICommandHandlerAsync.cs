using System.Collections.Generic;
using System.Threading.Tasks;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Infrastructure.Dispatcher
{
    public interface ICommandHandlerAsync<in TCommand> where TCommand : ICommand
    {
        Task<IEnumerable<IDomainEvent>> HandleAsync(TCommand command);
    }
}
