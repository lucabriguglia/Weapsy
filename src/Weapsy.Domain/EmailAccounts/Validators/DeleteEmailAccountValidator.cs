using FluentValidation;
using System;
using Weapsy.Domain.Model.EmailAccounts.Commands;
using Weapsy.Domain.Model.Sites.Rules;

namespace Weapsy.Domain.Model.EmailAccounts.Validators
{
    public class DeleteEmailAccountValidator : AbstractValidator<DeleteEmailAccount>
    {
        private readonly ISiteRules _siteRules;

        public DeleteEmailAccountValidator(ISiteRules siteRules)
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