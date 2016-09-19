using FluentValidation;
using Weapsy.Domain.Languages.Rules;
using Weapsy.Domain.Pages.Commands;
using Weapsy.Domain.Pages.Rules;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Pages.Validators
{
    public class UpdatePageDetailsValidator : PageDetailsValidator<UpdatePageDetails>
    {
        public UpdatePageDetailsValidator(IPageRules pageRules, 
            ISiteRules siteRules,
            ILanguageRules languageRules,
            IValidator<PageLocalisation> localisationValidator)
            : base(pageRules, siteRules, languageRules, localisationValidator)
        {
        }
    }
}