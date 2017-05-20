using FluentValidation;
using System;
using System.Collections.Generic;
using Weapsy.Domain.ModuleTypes.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.ModuleTypes.Handlers
{
    public class DeleteModuleTypeHandler : ICommandHandler<DeleteModuleType>
    {
        private readonly IModuleTypeRepository _moduleTypeRepository;
        private readonly IValidator<DeleteModuleType> _validator;

        public DeleteModuleTypeHandler(IModuleTypeRepository moduleTypeRepository, IValidator<DeleteModuleType> validator)
        {
            _moduleTypeRepository = moduleTypeRepository;
            _validator = validator;
        }

        public IEnumerable<IEvent> Handle(DeleteModuleType command)
        {
            var moduleType = _moduleTypeRepository.GetById(command.Id);

            if (moduleType == null)
                throw new Exception("ModuleType not found.");

            moduleType.Delete(command, _validator);

            _moduleTypeRepository.Update(moduleType);

            return moduleType.Events;
        }
    }
}
