using System.Collections.Generic;
using FluentValidation;
using Weapsy.Domain.Apps.Commands;
using System;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.Apps.Handlers
{
    public class UpdateAppDetailsHandler : ICommandHandler<UpdateAppDetailsCommand>
    {
        private readonly IAppRepository _repository;
        private readonly IValidator<UpdateAppDetailsCommand> _validator;

        public UpdateAppDetailsHandler(IAppRepository repository, IValidator<UpdateAppDetailsCommand> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public IEnumerable<IEvent> Handle(UpdateAppDetailsCommand cmd)
        {
            var app = _repository.GetById(cmd.Id);

            if (app == null)
                throw new Exception("App not found.");

            app.UpdateDetails(cmd, _validator);

            _repository.Update(app);

            return app.Events;
        }
    }
}
