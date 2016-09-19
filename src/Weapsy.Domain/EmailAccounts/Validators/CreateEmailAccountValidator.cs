using FluentValidation;
using System;
using Weapsy.Domain.Model.EmailAccounts.Commands;
using Weapsy.Domain.Model.EmailAccounts.Rules;
using Weapsy.Domain.Model.Sites.Rules;

namespace Weapsy.Domain.Model.EmailAccounts.Validators
{
    public class CreateEmailAccountValidator : EmailAccountDetailsValidator<CreateEmailAccount>
    {
        private readonly IEmailAccountRules _emailAccountRules;

        public CreateEmailAccountValidator(IEmailAccountRules emailAccountRules, ISiteRules siteRules)
            : base(emailAccountRules, siteRules)
        {
            _emailAccountRules = emailAccountRules;

            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Id is required.")
                .Must(HaveUniqueId).WithMessage("An email account with the same id already exists.");
        }

        private bool HaveUniqueId(Guid id)
        {
            return _emailAccountRules.IsEmailAccountIdUnique(id);
        }
    }
}
