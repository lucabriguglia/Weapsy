using System;
using FluentValidation;
using Weapsy.Domain.Model.ModuleTypes.Commands;
using Weapsy.Domain.Model.ModuleTypes.Rules;
using Weapsy.Domain.Model.Apps.Rules;

namespace Weapsy.Domain.Model.ModuleTypes.Validators
{
    public class CreateModuleTypeValidator : ModuleTypeDetailsValidator<CreateModuleType>
    {
        private readonly IModuleTypeRules _moduleTypeRules;
        private readonly IAppRules _appRules;

        public CreateModuleTypeValidator(IModuleTypeRules moduleTypeRules, IAppRules appRules)
            : base (moduleTypeRules)
        {
            _moduleTypeRules = moduleTypeRules;
            _appRules = appRules;

            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Id is required.")
                .Must(HaveUniqueId).WithMessage("A module type with the same id already exists.");

            RuleFor(c => c.AppId)
                .NotEmpty().WithMessage("App id is required.")
                .Must(BeAnExistingApp).WithMessage("App does not exist.");
        }

        private bool HaveUniqueId(Guid id)
        {
            return _moduleTypeRules.IsModuleTypeIdUnique(id);
        }

        private bool BeAnExistingApp(Guid appId)
        {
            return _appRules.DoesAppExist(appId);
        }
    }
}
