using Weapsy.Domain.Pages.Commands;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Pages.Validators
{
    public class HidePageValidator : BaseSiteValidator<HidePage>
    {
        public HidePageValidator(ISiteRules siteRules)
            : base(siteRules)
        {
        }
    }
}