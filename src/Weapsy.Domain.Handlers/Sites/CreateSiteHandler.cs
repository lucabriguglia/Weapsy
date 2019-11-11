using System.Threading.Tasks;
using Kledex.Commands;
using Weapsy.Domain.Models.Sites;
using Weapsy.Domain.Models.Sites.Commands;

namespace Weapsy.Domain.Handlers.Sites
{
    public class CreateSiteHandler : ICommandHandlerAsync<CreateSite>
    {
        private readonly ISiteRepository _repository;

        public CreateSiteHandler(ISiteRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse> HandleAsync(CreateSite command)
        {
            var site = new Site(command);

            await _repository.CreateAsync(site);

            return new CommandResponse
            {
                Events = site.Events
            };
        }
    }
}
