using System.Collections.Generic;
using FluentValidation;
using Weapsy.Domain.Modules.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.Modules.Handlers
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

        public IEnumerable<IEvent> Handle(CreateModule command)
        {
            var module = Module.CreateNew(command, _validator);

            _moduleRepository.Create(module);

            return module.Events;
        }
    }
}
