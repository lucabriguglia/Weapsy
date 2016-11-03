using System;
using FluentValidation;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain
{
    public class BaseSiteValidator<T> : AbstractValidator<T> where T : BaseSiteCommand
    {
        private readonly ISiteRules _siteRules;

        public BaseSiteValidator(ISiteRules siteRules)
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
