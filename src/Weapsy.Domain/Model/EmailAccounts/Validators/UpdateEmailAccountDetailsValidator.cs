using Weapsy.Domain.Model.EmailAccounts.Commands;
using Weapsy.Domain.Model.EmailAccounts.Rules;
using Weapsy.Domain.Model.Sites.Rules;

namespace Weapsy.Domain.Model.EmailAccounts.Validators
{
    public class UpdateEmailAccountDetailsValidator : EmailAccountDetailsValidator<UpdateEmailAccountDetails>
    {
        public UpdateEmailAccountDetailsValidator(IEmailAccountRules emailAccountRules, ISiteRules siteRules)
            : base(emailAccountRules, siteRules)
        {
        }
    }
}
