using Weapsy.Domain.Pages.Commands;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Pages.Validators
{
    public class DeletePageValidator : BaseSiteValidator<DeletePage>
    {
        public DeletePageValidator(ISiteRules siteRules)
            : base(siteRules)
        {
        }
    }
}