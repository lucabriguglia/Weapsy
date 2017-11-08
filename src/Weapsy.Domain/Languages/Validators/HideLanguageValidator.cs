using Weapsy.Domain.Languages.Commands;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Languages.Validators
{
    public class HideLanguageValidator : BaseSiteValidator<HideLanguageCommand>
    {
        public HideLanguageValidator(ISiteRules siteRules)
            : base(siteRules)
        {
        }
    }
}