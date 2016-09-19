using FluentValidation;
using System;
using Weapsy.Domain.Pages.Commands;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Pages.Validators
{
    public class RemovePageModuleValidator : AbstractValidator<RemovePageModule>
    {
        private readonly ISiteRules _siteRules;

        public RemovePageModuleValidator(ISiteRules siteRules)
        {
            _siteRules = siteRules;

            RuleFor(c => c.SiteId)
                .NotEmpty().WithMessage("Site id is required.")
                .Must(BeAnExistingSite).WithMessage("Site does not exist.");
        }

        private bool BeAnExistingSite(Guid siteId)
        {
            return _siteRules.DoesSiteExist(siteId);
        }
    }
}