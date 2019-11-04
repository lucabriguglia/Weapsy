using System.Threading.Tasks;
using FluentValidation;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.Apps.Commands.Handlers
{
    public class CreateAppHandler : ICommandHandlerWithAggregateAsync<CreateApp>
    {
        private readonly IAppRepository _appRepository;
        private readonly IValidator<CreateApp> _validator;

        public CreateAppHandler(IAppRepository appRepository,
            IValidator<CreateApp> validator)
        {
            _appRepository = appRepository;
            _validator = validator;
        }

        public async Task<IAggregateRoot> HandleAsync(CreateApp command)
        {
            await _validator.ValidateCommandAsync(command);

            var app = new App(command);

            await _appRepository.CreateAsync(app);

            return app;
        }
    }
}
