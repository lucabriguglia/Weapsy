using System.Collections.Generic;
using FluentValidation;
using Weapsy.Domain.Templates.Commands;
using System;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.Templates.Handlers
{
    public class UpdateTemplateDetailsHandler : ICommandHandler<UpdateTemplateDetailsCommand>
    {
        private readonly ITemplateRepository _templateRepository;
        private readonly IValidator<UpdateTemplateDetailsCommand> _validator;

        public UpdateTemplateDetailsHandler(ITemplateRepository templateRepository, IValidator<UpdateTemplateDetailsCommand> validator)
        {
            _templateRepository = templateRepository;
            _validator = validator;
        }

        public IEnumerable<IEvent> Handle(UpdateTemplateDetailsCommand cmd)
        {
            var template = _templateRepository.GetById(cmd.Id);

            if (template == null)
                throw new Exception("Template not found.");

            template.UpdateDetails(cmd, _validator);

            _templateRepository.Update(template);

            return template.Events;
        }
    }
}
