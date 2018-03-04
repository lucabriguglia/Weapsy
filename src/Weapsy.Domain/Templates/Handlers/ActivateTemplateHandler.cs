using System;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;
using Weapsy.Domain.Templates.Commands;

namespace Weapsy.Domain.Templates.Handlers
{
    public class ActivateTemplateHandler : ICommandHandlerWithAggregate<ActivateTemplate>
    {
        private readonly ITemplateRepository _templateRepository;

        public ActivateTemplateHandler(ITemplateRepository templateRepository)
        {
            _templateRepository = templateRepository;
        }

        public IAggregateRoot Handle(ActivateTemplate command)
        {
            var template = _templateRepository.GetById(command.Id);

            if (template == null)
                throw new Exception("Template not found.");

            template.Activate();

            _templateRepository.Update(template);

            return template;
        }
    }
}
