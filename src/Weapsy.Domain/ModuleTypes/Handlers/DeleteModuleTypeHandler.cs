using FluentValidation;
using System;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;
using Weapsy.Domain.ModuleTypes.Commands;

namespace Weapsy.Domain.ModuleTypes.Handlers
{
    public class DeleteModuleTypeHandler : ICommandHandlerWithAggregate<DeleteModuleType>
    {
        private readonly IModuleTypeRepository _moduleTypeRepository;
        private readonly IValidator<DeleteModuleType> _validator;

        public DeleteModuleTypeHandler(IModuleTypeRepository moduleTypeRepository, IValidator<DeleteModuleType> validator)
        {
            _moduleTypeRepository = moduleTypeRepository;
            _validator = validator;
        }

        public IAggregateRoot Handle(DeleteModuleType command)
        {
            var moduleType = _moduleTypeRepository.GetById(command.Id);

            if (moduleType == null)
                throw new Exception("ModuleType not found.");

            moduleType.Delete(command, _validator);

            _moduleTypeRepository.Update(moduleType);

            return moduleType;
        }
    }
}
