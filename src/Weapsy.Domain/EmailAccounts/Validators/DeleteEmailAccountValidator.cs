using Weapsy.Domain.EmailAccounts.Commands;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.EmailAccounts.Validators
{
    public class DeleteEmailAccountValidator : BaseSiteValidator<DeleteEmailAccount>
    {
        public DeleteEmailAccountValidator(ISiteRules siteRules)
            : base(siteRules)
        {
        }
    }
}