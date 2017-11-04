using System.Collections.Generic;
using FluentValidation;
using Weapsy.Domain.ModuleTypes.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.ModuleTypes.Handlers
{
    public class CreateModuleTypeHandler : ICommandHandler<CreateModuleTypeCommand>
    {
        private readonly IModuleTypeRepository _moduleTypeRepository;
        private readonly IValidator<CreateModuleTypeCommand> _validator;

        public CreateModuleTypeHandler(IModuleTypeRepository moduleTypeRepository, IValidator<CreateModuleTypeCommand> validator)
        {
            _moduleTypeRepository = moduleTypeRepository;
            _validator = validator;
        }

        public IEnumerable<IEvent> Handle(CreateModuleTypeCommand command)
        {
            var moduleType = ModuleType.CreateNew(command, _validator);

            _moduleTypeRepository.Create(moduleType);

            return moduleType.Events;
        }
    }
}
