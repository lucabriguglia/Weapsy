using FluentValidation;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;
using Weapsy.Domain.Modules.Commands;

namespace Weapsy.Domain.Modules.Handlers
{
    public class CreateModuleHandler : ICommandHandlerWithAggregate<CreateModule>
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly IValidator<CreateModule> _validator;

        public CreateModuleHandler(IModuleRepository moduleRepository, IValidator<CreateModule> validator)
        {
            _moduleRepository = moduleRepository;
            _validator = validator;
        }

        public IAggregateRoot Handle(CreateModule command)
        {
            var module = Module.CreateNew(command, _validator);

            _moduleRepository.Create(module);

            return module;
        }
    }
}
