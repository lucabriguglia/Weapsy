using Weapsy.Domain.Languages.Commands;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Languages.Validators
{
    public class ActivateLanguageValidator : BaseSiteValidator<ActivateLanguageCommand>
    {
        public ActivateLanguageValidator(ISiteRules siteRules)
            : base(siteRules)
        {
        }
    }
}