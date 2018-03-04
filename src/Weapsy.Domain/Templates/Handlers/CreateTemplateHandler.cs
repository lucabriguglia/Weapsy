using FluentValidation;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;
using Weapsy.Domain.Templates.Commands;

namespace Weapsy.Domain.Templates.Handlers
{
    public class CreateTemplateHandler : ICommandHandlerWithAggregate<CreateTemplate>
    {
        private readonly ITemplateRepository _templateRepository;
        private readonly IValidator<CreateTemplate> _validator;

        public CreateTemplateHandler(ITemplateRepository templateRepository,
            IValidator<CreateTemplate> validator)
        {
            _templateRepository = templateRepository;
            _validator = validator;
        }

        public IAggregateRoot Handle(CreateTemplate command)
        {
            var template = Template.CreateNew(command, _validator);

            _templateRepository.Create(template);

            return template;
        }
    }
}
