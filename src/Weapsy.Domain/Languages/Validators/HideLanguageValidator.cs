using Weapsy.Domain.Languages.Commands;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Languages.Validators
{
    public class HideLanguageValidator : BaseSiteValidator<HideLanguage>
    {
        public HideLanguageValidator(ISiteRules siteRules)
            : base(siteRules)
        {
        }
    }
}