using System;
using FluentValidation;
using Weapsy.Domain.Model.ModuleTypes.Commands;
using Weapsy.Domain.Model.ModuleTypes.Rules;

namespace Weapsy.Domain.Model.ModuleTypes.Validators
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
