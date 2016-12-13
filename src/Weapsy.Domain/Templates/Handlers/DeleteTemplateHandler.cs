using System;
using System.Collections.Generic;
using Weapsy.Infrastructure.Domain;
using Weapsy.Domain.Templates.Commands;

namespace Weapsy.Domain.Templates.Handlers
{
    public class DeleteTemplateHandler : ICommandHandler<DeleteTemplate>
    {
        private readonly ITemplateRepository _templateRepository;

        public DeleteTemplateHandler(ITemplateRepository templateRepository)
        {
            _templateRepository = templateRepository;
        }

        public IEnumerable<IDomainEvent> Handle(DeleteTemplate command)
        {
            var template = _templateRepository.GetById(command.Id);

            if (template == null)
                throw new Exception("Template not found.");

            template.Delete();

            _templateRepository.Update(template);

            return template.Events;
        }
    }
}
