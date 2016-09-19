using System;
using FluentValidation;
using Weapsy.Domain.Modules.Commands;
using Weapsy.Domain.Modules.Rules;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Modules.Validators
{
    public class DeleteModuleValidator : AbstractValidator<DeleteModule>
    {
        private readonly IModuleRules _moduleRules;
        private readonly ISiteRules _siteRules;

        public DeleteModuleValidator(ISiteRules siteRules, IModuleRules moduleRules)
        {
            _siteRules = siteRules;
            _moduleRules = moduleRules;

            RuleFor(c => c.SiteId)
                .NotEmpty().WithMessage("Site id is required.")
                .Must(BeAnExistingSite).WithMessage("Site does not exist.");

            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Id is required.")
                .Must(NotBeInUse).WithMessage("Module is in use.");
        }

        private bool BeAnExistingSite(Guid siteId)
        {
            return _siteRules.DoesSiteExist(siteId);
        }

        private bool NotBeInUse(Guid id)
        {
            return !_moduleRules.IsModuleInUse(id);
        }
    }
}
