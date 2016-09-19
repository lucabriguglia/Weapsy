using FluentValidation;
using System;
using Weapsy.Domain.Menus.Commands;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Menus.Validators
{
    public class RemoveMenuItemValidator : AbstractValidator<RemoveMenuItem>
    {
        private readonly ISiteRules _siteRules;

        public RemoveMenuItemValidator(ISiteRules siteRules)
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