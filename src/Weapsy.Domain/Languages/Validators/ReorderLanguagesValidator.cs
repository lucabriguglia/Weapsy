using Weapsy.Domain.Languages.Commands;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Languages.Validators
{
    public class ReorderLanguagesValidator : BaseSiteValidator<ReorderLanguagesCommand>
    {
        public ReorderLanguagesValidator(ISiteRules siteRules)
            : base(siteRules)
        {
        }
    }
}