using FluentValidation;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;
using Weapsy.Domain.ModuleTypes.Commands;

namespace Weapsy.Domain.ModuleTypes.Handlers
{
    public class CreateModuleTypeHandler : ICommandHandlerWithAggregate<CreateModuleType>
    {
        private readonly IModuleTypeRepository _moduleTypeRepository;
        private readonly IValidator<CreateModuleType> _validator;

        public CreateModuleTypeHandler(IModuleTypeRepository moduleTypeRepository, IValidator<CreateModuleType> validator)
        {
            _moduleTypeRepository = moduleTypeRepository;
            _validator = validator;
        }

        public IAggregateRoot Handle(CreateModuleType command)
        {
            var moduleType = ModuleType.CreateNew(command, _validator);

            _moduleTypeRepository.Create(moduleType);

            return moduleType;
        }
    }
}
