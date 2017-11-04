using System.Collections.Generic;
using FluentValidation;
using Weapsy.Domain.Apps.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.Apps.Handlers
{
    public class CreateAppHandler : ICommandHandler<CreateAppCommand>
    {
        private readonly IAppRepository _appRepository;
        private readonly IValidator<CreateAppCommand> _validator;

        public CreateAppHandler(IAppRepository appRepository,
            IValidator<CreateAppCommand> validator)
        {
            _appRepository = appRepository;
            _validator = validator;
        }

        public IEnumerable<IEvent> Handle(CreateAppCommand command)
        {
            var app = App.CreateNew(command, _validator);

            _appRepository.Create(app);

            return app.Events;
        }
    }
}
