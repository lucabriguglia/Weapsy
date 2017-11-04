using Weapsy.Domain.Pages.Commands;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Pages.Validators
{
    public class ReorderPageModulesValidator : BaseSiteValidator<ReorderPageModulesCommand>
    {
        public ReorderPageModulesValidator(ISiteRules siteRules)
            : base(siteRules)
        {
        }
    }
}