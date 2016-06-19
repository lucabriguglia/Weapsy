using System.Collections.Generic;
using FluentValidation;
using Weapsy.Core.Domain;
using Weapsy.Domain.Model.Modules.Commands;

namespace Weapsy.Domain.Model.Modules.Handlers
{
    public class CreateModuleHandler : ICommandHandler<CreateModule>
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly IValidator<CreateModule> _validator;

        public CreateModuleHandler(IModuleRepository moduleRepository, IValidator<CreateModule> validator)
        {
            _moduleRepository = moduleRepository;
            _validator = validator;
        }

        public ICollection<IEvent> Handle(CreateModule command)
        {
            var module = Module.CreateNew(command, _validator);

            _moduleRepository.Create(module);

            return module.Events;
        }
    }
}
