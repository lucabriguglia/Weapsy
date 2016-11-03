using System;
using System.Collections.Generic;
using Weapsy.Infrastructure.Domain;
using Weapsy.Domain.Templates.Commands;

namespace Weapsy.Domain.Templates.Handlers
{
    public class ActivateTemplateHandler : ICommandHandler<ActivateTemplate>
    {
        private readonly ITemplateRepository _templateRepository;

        public ActivateTemplateHandler(ITemplateRepository templateRepository)
        {
            _templateRepository = templateRepository;
        }

        public ICollection<IEvent> Handle(ActivateTemplate command)
        {
            var template = _templateRepository.GetById(command.Id);

            if (template == null)
                throw new Exception("Template not found.");

            template.Activate();

            _templateRepository.Update(template);

            return template.Events;
        }
    }
}
