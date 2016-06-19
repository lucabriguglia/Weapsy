using FluentValidation;
using Weapsy.Domain.Model.Languages.Rules;
using Weapsy.Domain.Model.Pages.Commands;
using Weapsy.Domain.Model.Pages.Rules;
using Weapsy.Domain.Model.Sites.Rules;

namespace Weapsy.Domain.Model.Pages.Validators
{
    public class UpdatePageDetailsValidator : PageDetailsValidator<UpdatePageDetails>
    {
        public UpdatePageDetailsValidator(IPageRules pageRules, 
            ISiteRules siteRules,
            ILanguageRules languageRules,
            IValidator<PageDetails.PageLocalisation> localisationValidator)
            : base(pageRules, siteRules, languageRules, localisationValidator)
        {
        }
    }
}