using System.Collections.Generic;
using FluentValidation;
using Weapsy.Core.Domain;
using Weapsy.Domain.Model.ModuleTypes.Commands;
using System;

namespace Weapsy.Domain.Model.ModuleTypes.Handlers
{
    public class UpdateModuleTypeDetailsHandler : ICommandHandler<UpdateModuleTypeDetails>
    {
        private readonly IModuleTypeRepository _moduleTypeRepository;
        private readonly IValidator<UpdateModuleTypeDetails> _validator;

        public UpdateModuleTypeDetailsHandler(IModuleTypeRepository moduleTypeRepository,
            IValidator<UpdateModuleTypeDetails> validator)
        {
            _moduleTypeRepository = moduleTypeRepository;
            _validator = validator;
        }

        public ICollection<IEvent> Handle(UpdateModuleTypeDetails command)
        {
            var page = _moduleTypeRepository.GetById(command.Id);

            if (page == null)
                throw new Exception("Module Type not found");

            page.UpdateDetails(command, _validator);

            _moduleTypeRepository.Update(page);

            return page.Events;
        }
    }
}
