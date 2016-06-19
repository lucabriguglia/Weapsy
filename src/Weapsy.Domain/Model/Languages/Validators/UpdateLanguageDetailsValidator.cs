using Weapsy.Domain.Model.Languages.Commands;
using Weapsy.Domain.Model.Languages.Rules;
using Weapsy.Domain.Model.Sites.Rules;

namespace Weapsy.Domain.Model.Languages.Validators
{
    public class UpdateLanguageDetailsValidator : LanguageDetailsValidator<UpdateLanguageDetails>
    {
        public UpdateLanguageDetailsValidator(ILanguageRules languageRules, ISiteRules siteRules)
            : base (languageRules, siteRules)
        {
        }
    }
}
