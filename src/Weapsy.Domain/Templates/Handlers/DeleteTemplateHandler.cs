using System;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;
using Weapsy.Domain.Templates.Commands;

namespace Weapsy.Domain.Templates.Handlers
{
    public class DeleteTemplateHandler : ICommandHandlerWithAggregate<DeleteTemplate>
    {
        private readonly ITemplateRepository _templateRepository;

        public DeleteTemplateHandler(ITemplateRepository templateRepository)
        {
            _templateRepository = templateRepository;
        }

        public IAggregateRoot Handle(DeleteTemplate command)
        {
            var template = _templateRepository.GetById(command.Id);

            if (template == null)
                throw new Exception("Template not found.");

            template.Delete();

            _templateRepository.Update(template);

            return template;
        }
    }
}
