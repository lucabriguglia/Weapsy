using Weapsy.Domain.Languages.Commands;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Languages.Validators
{
    public class ActivateLanguageValidator : BaseSiteValidator<ActivateLanguage>
    {
        public ActivateLanguageValidator(ISiteRules siteRules)
            : base(siteRules)
        {
        }
    }
}