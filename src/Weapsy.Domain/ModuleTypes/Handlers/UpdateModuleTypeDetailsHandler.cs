using FluentValidation;
using Weapsy.Domain.ModuleTypes.Commands;
using System;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.ModuleTypes.Handlers
{
    public class UpdateModuleTypeDetailsHandler : ICommandHandlerWithAggregate<UpdateModuleTypeDetails>
    {
        private readonly IModuleTypeRepository _moduleTypeRepository;
        private readonly IValidator<UpdateModuleTypeDetails> _validator;

        public UpdateModuleTypeDetailsHandler(IModuleTypeRepository moduleTypeRepository,
            IValidator<UpdateModuleTypeDetails> validator)
        {
            _moduleTypeRepository = moduleTypeRepository;
            _validator = validator;
        }

        public IAggregateRoot Handle(UpdateModuleTypeDetails command)
        {
            var moduleType = _moduleTypeRepository.GetById(command.Id);

            if (moduleType == null)
                throw new Exception("Module Type not found");

            moduleType.UpdateDetails(command, _validator);

            _moduleTypeRepository.Update(moduleType);

            return moduleType;
        }
    }
}
