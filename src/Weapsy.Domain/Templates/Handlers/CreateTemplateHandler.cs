using System.Collections.Generic;
using FluentValidation;
using Weapsy.Infrastructure.Domain;
using Weapsy.Domain.Templates.Commands;
using Weapsy.Infrastructure.Dispatcher;

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

        public IEnumerable<IDomainEvent> Handle(CreateTemplate command)
        {
            var template = Template.CreateNew(command, _validator);

            _templateRepository.Create(template);

            return template.Events;
        }
    }
}
