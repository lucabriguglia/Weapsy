using System;
using FluentValidation;
using Weapsy.Domain.ModuleTypes.Commands;
using Weapsy.Domain.ModuleTypes.Rules;
using Weapsy.Domain.Apps.Rules;

namespace Weapsy.Domain.ModuleTypes.Validators
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
                .Must(HaveUniqueId).WithMessage("A module type with the same id already exists.")
                .When(x => x.Id != Guid.Empty);

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
