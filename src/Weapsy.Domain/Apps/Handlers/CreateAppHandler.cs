using FluentValidation;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;
using Weapsy.Domain.Apps.Commands;

namespace Weapsy.Domain.Apps.Handlers
{
    public class CreateAppHandler : ICommandHandlerWithAggregate<CreateApp>
    {
        private readonly IAppRepository _appRepository;
        private readonly IValidator<CreateApp> _validator;

        public CreateAppHandler(IAppRepository appRepository,
            IValidator<CreateApp> validator)
        {
            _appRepository = appRepository;
            _validator = validator;
        }

        public IAggregateRoot Handle(CreateApp command)
        {
            var app = App.CreateNew(command, _validator);

            _appRepository.Create(app);

            return app;
        }
    }
}
