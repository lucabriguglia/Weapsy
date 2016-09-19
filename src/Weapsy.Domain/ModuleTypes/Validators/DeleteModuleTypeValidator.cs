using System;
using FluentValidation;
using Weapsy.Domain.ModuleTypes.Commands;
using Weapsy.Domain.ModuleTypes.Rules;

namespace Weapsy.Domain.ModuleTypes.Validators
{
    public class DeleteModuleTypeValidator : AbstractValidator<DeleteModuleType>
    {
        private readonly IModuleTypeRules _moduleTypeRules;

        public DeleteModuleTypeValidator(IModuleTypeRules moduleTypeRules)
        {
            _moduleTypeRules = moduleTypeRules;

            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Id is required.")
                .Must(NotBeInUse).WithMessage("Module type is in use.");
        }

        private bool NotBeInUse(Guid id)
        {
            return !_moduleTypeRules.IsModuleTypeInUse(id);
        }
    }
}
