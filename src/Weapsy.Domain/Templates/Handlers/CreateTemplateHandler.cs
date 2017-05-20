using System.Collections.Generic;
using FluentValidation;
using Weapsy.Domain.Templates.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.Templates.Handlers
{
    public class CreateTemplateHandler : ICommandHandler<CreateTemplate>
    {
        private readonly ITemplateRepository _templateRepository;
        private readonly IValidator<CreateTemplate> _validator;

        public CreateTemplateHandler(ITemplateRepository templateRepository,
            IValidator<CreateTemplate> validator)
        {
            _templateRepository = templateRepository;
            _validator = validator;
        }

        public IEnumerable<IEvent> Handle(CreateTemplate command)
        {
            var template = Template.CreateNew(command, _validator);

            _templateRepository.Create(template);

            return template.Events;
        }
    }
}
