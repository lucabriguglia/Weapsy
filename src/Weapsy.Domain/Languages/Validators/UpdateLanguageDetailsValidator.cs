using Weapsy.Domain.Languages.Commands;
using Weapsy.Domain.Languages.Rules;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Languages.Validators
{
    public class UpdateLanguageDetailsValidator : LanguageDetailsValidator<UpdateLanguageDetails>
    {
        public UpdateLanguageDetailsValidator(ILanguageRules languageRules, ISiteRules siteRules)
            : base (languageRules, siteRules)
        {
        }
    }
}
