using System;
using FluentValidation;
using Weapsy.Domain.Modules.Commands;
using Weapsy.Domain.Modules.Rules;
using Weapsy.Domain.Sites.Rules;
using Weapsy.Domain.ModuleTypes.Rules;

namespace Weapsy.Domain.Modules.Validators
{
    public class CreateModuleValidator : AbstractValidator<CreateModule>
    {
        private readonly IModuleRules _moduleRules;
        private readonly IModuleTypeRules _moduleTypeRules;
        private readonly ISiteRules _siteRules;

        public CreateModuleValidator(IModuleRules moduleRules, IModuleTypeRules moduleTypeRules, ISiteRules siteRules)
        {
            _moduleRules = moduleRules;
            _moduleTypeRules = moduleTypeRules;
            _siteRules = siteRules;

            RuleFor(c => c.Id)
                .Must(HaveUniqueId).WithMessage("A module with the same id already exists.")
                .When(x => x.Id != Guid.Empty);

            RuleFor(c => c.SiteId)
                .NotEmpty().WithMessage("Site id is required.")
                .Must(BeAnExistingSite).WithMessage("Site does not exist.");

            RuleFor(c => c.ModuleTypeId)
                .NotEmpty().WithMessage("Module type id is required.")
                .Must(BeAnExistingModuleType).WithMessage("Module type does not exist.");

            RuleFor(c => c.Title)
                .NotEmpty().WithMessage("Module title is required.")
                .Length(1, 100).WithMessage("Module title length must be between 1 and 100 characters.");
        }

        private bool HaveUniqueId(Guid id)
        {
            return _moduleRules.IsModuleIdUnique(id);
        }

        private bool BeAnExistingSite(Guid siteId)
        {
            return _siteRules.DoesSiteExist(siteId);
        }

        private bool BeAnExistingModuleType(Guid moduleTypeId)
        {
            return _moduleTypeRules.DoesModuleTypeExist(moduleTypeId);
        }
    }
}
