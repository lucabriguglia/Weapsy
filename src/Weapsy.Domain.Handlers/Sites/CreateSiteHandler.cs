using System.Threading.Tasks;
using Kledex.Commands;
using Weapsy.Domain.Models.Sites.Commands;

namespace Weapsy.Domain.Handlers
{
    public class CreateSiteHandler : ICommandHandlerAsync<CreateSite>
    {
        public Task<CommandResponse> HandleAsync(CreateSite command)
        {
            throw new System.NotImplementedException();
        }
    }
}
