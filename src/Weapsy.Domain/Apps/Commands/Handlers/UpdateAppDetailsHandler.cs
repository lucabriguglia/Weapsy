using System;
using System.Threading.Tasks;
using FluentValidation;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.Apps.Commands.Handlers
{
    public class UpdateAppDetailsHandler : ICommandHandlerWithAggregateAsync<UpdateAppDetails>
    {
        private readonly IAppRepository _repository;
        private readonly IValidator<UpdateAppDetails> _validator;

        public UpdateAppDetailsHandler(IAppRepository repository, IValidator<UpdateAppDetails> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<IAggregateRoot> HandleAsync(UpdateAppDetails command)
        {
            var app = _repository.GetById(command.Id);

            if (app == null)
                throw new ApplicationException("App not found.");

            await _validator.ValidateCommandAsync(command);

            app.UpdateDetails(command);

            await _repository.UpdateAsync(app);

            return app;
        }
    }
}
