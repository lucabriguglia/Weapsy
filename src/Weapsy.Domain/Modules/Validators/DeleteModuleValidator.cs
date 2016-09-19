using System;
using FluentValidation;
using Weapsy.Domain.Model.Modules.Commands;
using Weapsy.Domain.Model.Modules.Rules;
using Weapsy.Domain.Model.Sites.Rules;

namespace Weapsy.Domain.Model.Modules.Validators
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
