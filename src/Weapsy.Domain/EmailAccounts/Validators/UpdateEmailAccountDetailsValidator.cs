using Weapsy.Domain.EmailAccounts.Commands;
using Weapsy.Domain.EmailAccounts.Rules;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.EmailAccounts.Validators
{
    public class UpdateEmailAccountDetailsValidator : EmailAccountDetailsValidator<UpdateEmailAccountDetails>
    {
        public UpdateEmailAccountDetailsValidator(IEmailAccountRules emailAccountRules, ISiteRules siteRules)
            : base(emailAccountRules, siteRules)
        {
        }
    }
}
