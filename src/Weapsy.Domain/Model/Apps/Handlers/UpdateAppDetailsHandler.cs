using System.Collections.Generic;
using FluentValidation;
using Weapsy.Core.Domain;
using Weapsy.Domain.Model.Apps.Commands;
using System;

namespace Weapsy.Domain.Model.Apps.Handlers
{
    public class UpdateAppDetailsHandler : ICommandHandler<UpdateAppDetails>
    {
        private readonly IAppRepository _appRepository;
        private readonly IValidator<UpdateAppDetails> _validator;

        public UpdateAppDetailsHandler(IAppRepository appRepository, IValidator<UpdateAppDetails> validator)
        {
            _appRepository = appRepository;
            _validator = validator;
        }

        public ICollection<IEvent> Handle(UpdateAppDetails cmd)
        {
            var app = _appRepository.GetById(cmd.Id);

            if (app == null)
                throw new Exception("App not found.");

            app.UpdateDetails(cmd, _validator);

            _appRepository.Update(app);

            return app.Events;
        }
    }
}
