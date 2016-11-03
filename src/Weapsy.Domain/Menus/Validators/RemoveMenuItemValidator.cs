using Weapsy.Domain.Menus.Commands;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Menus.Validators
{
    public class RemoveMenuItemValidator : BaseSiteValidator<RemoveMenuItem>
    {
        public RemoveMenuItemValidator(ISiteRules siteRules)
            : base(siteRules)
        {
        }
    }
}