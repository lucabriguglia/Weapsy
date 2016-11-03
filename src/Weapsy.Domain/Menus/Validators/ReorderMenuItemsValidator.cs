using Weapsy.Domain.Menus.Commands;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Menus.Validators
{
    public class ReorderMenuItemsValidator : BaseSiteValidator<ReorderMenuItems>
    {
        public ReorderMenuItemsValidator(ISiteRules siteRules)
            : base(siteRules)
        {
        }
    }
}