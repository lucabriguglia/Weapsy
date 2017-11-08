using System;
using System.Collections.Generic;
using Weapsy.Domain.Templates.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.Templates.Handlers
{
    public class DeleteTemplateHandler : ICommandHandler<DeleteTemplateCommand>
    {
        private readonly ITemplateRepository _templateRepository;

        public DeleteTemplateHandler(ITemplateRepository templateRepository)
        {
            _templateRepository = templateRepository;
        }

        public IEnumerable<IEvent> Handle(DeleteTemplateCommand command)
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
