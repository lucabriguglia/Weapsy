using Weapsy.Domain.Pages.Commands;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Pages.Validators
{
    public class ActivatePageValidator : BaseSiteValidator<ActivatePageCommand>
    {
        public ActivatePageValidator(ISiteRules siteRules)
            : base(siteRules)
        {
        }
    }
}