using FluentValidation;
using System;
using Weapsy.Domain.Languages.Commands;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Languages.Validators
{
    public class ReorderLanguagesValidator : AbstractValidator<ReorderLanguages>
    {
        private readonly ISiteRules _siteRules;

        public ReorderLanguagesValidator(ISiteRules siteRules)
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