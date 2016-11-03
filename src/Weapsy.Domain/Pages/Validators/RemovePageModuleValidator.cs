using Weapsy.Domain.Pages.Commands;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Pages.Validators
{
    public class RemovePageModuleValidator : BaseSiteValidator<RemovePageModule>
    {
        public RemovePageModuleValidator(ISiteRules siteRules)
            : base(siteRules)
        {
        }
    }
}