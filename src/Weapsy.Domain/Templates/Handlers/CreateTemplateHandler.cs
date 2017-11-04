using System.Collections.Generic;
using FluentValidation;
using Weapsy.Domain.Templates.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.Templates.Handlers
{
    public class CreateTemplateHandler : ICommandHandler<CreateTemplateCommand>
    {
        private readonly ITemplateRepository _templateRepository;
        private readonly IValidator<CreateTemplateCommand> _validator;

        public CreateTemplateHandler(ITemplateRepository templateRepository,
            IValidator<CreateTemplateCommand> validator)
        {
            _templateRepository = templateRepository;
            _validator = validator;
        }

        public IEnumerable<IEvent> Handle(CreateTemplateCommand command)
        {
            var template = Template.CreateNew(command, _validator);

            _templateRepository.Create(template);

            return template.Events;
        }
    }
}
