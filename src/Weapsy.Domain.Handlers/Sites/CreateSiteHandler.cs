using System.Threading.Tasks;
using FluentValidation;
using Kledex.Commands;
using Weapsy.Domain.Models.Sites;
using Weapsy.Domain.Models.Sites.Commands;

namespace Weapsy.Domain.Handlers.Sites
{
    public class CreateSiteHandler : ICommandHandlerAsync<CreateSite>
    {
        private readonly ISiteRepository _repository;
        private readonly IValidator<CreateSite> _validator;

        public CreateSiteHandler(ISiteRepository repository, IValidator<CreateSite> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<CommandResponse> HandleAsync(CreateSite command)
        {
            await _validator.ValidateCommandAsync(command);

            var site = new Site(command);

            await _repository.CreateAsync(site);

            return new CommandResponse
            {
                Events = site.Events
            };
        }
    }
}
