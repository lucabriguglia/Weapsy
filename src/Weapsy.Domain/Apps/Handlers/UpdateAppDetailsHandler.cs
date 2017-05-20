using System.Collections.Generic;
using FluentValidation;
using Weapsy.Domain.Apps.Commands;
using System;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.Apps.Handlers
{
    public class UpdateAppDetailsHandler : ICommandHandler<UpdateAppDetails>
    {
        private readonly IAppRepository _repository;
        private readonly IValidator<UpdateAppDetails> _validator;

        public UpdateAppDetailsHandler(IAppRepository repository, IValidator<UpdateAppDetails> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public IEnumerable<IEvent> Handle(UpdateAppDetails cmd)
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
