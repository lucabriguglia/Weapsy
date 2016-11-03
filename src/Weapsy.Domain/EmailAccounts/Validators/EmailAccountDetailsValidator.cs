using FluentValidation;
using Weapsy.Domain.EmailAccounts.Commands;
using Weapsy.Domain.EmailAccounts.Rules;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.EmailAccounts.Validators
{
    public class EmailAccountDetailsValidator<T> : BaseSiteValidator<T> where T : EmailAccountDetails
    {
        private readonly IEmailAccountRules _emailAccountRules;

        public EmailAccountDetailsValidator(IEmailAccountRules emailAccountRules, ISiteRules siteRules)
            : base(siteRules)
        {
            _emailAccountRules = emailAccountRules;

            RuleFor(c => c.Address)
                .NotEmpty().WithMessage("Email account address is required.")
                .Length(1, 250).WithMessage("Email account address length must be between 1 and 250 characters.")
                .EmailAddress().WithMessage("Email account address not valid.")
                .Must(HaveUniqueAddress).WithMessage("An email account with the same address already exists.");

            RuleFor(c => c.DisplayName)
                .Length(1, 100).WithMessage("Display name length must be between 1 and 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.DisplayName));

            RuleFor(c => c.Host)
                .NotEmpty().WithMessage("Host is required.")
                .Length(1, 250).WithMessage("Host length must be between 1 and 250 characters.");

            RuleFor(c => c.Port)
                .NotEmpty().WithMessage("Port is required.");

            RuleFor(c => c.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .Length(1, 250).WithMessage("Username length must be between 1 and 250 characters.")
                .When(x => x.DefaultCredentials == false);

            RuleFor(c => c.Password)
                .NotEmpty().WithMessage("Password is required.")
                .Length(1, 250).WithMessage("Password length must be between 1 and 250 characters.")
                .When(x => x.DefaultCredentials == false);
        }

        private bool HaveUniqueAddress(EmailAccountDetails cmd, string name)
        {
            return _emailAccountRules.IsEmailAccountAddressUnique(cmd.SiteId, name);
        }
    }
}
