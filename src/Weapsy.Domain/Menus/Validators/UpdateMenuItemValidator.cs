using FluentValidation;
using Weapsy.Domain.Languages.Rules;
using Weapsy.Domain.Menus.Commands;
using Weapsy.Domain.Pages.Rules;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Menus.Validators
{
    public class UpdateMenuItemValidator : MenuItemValidator<UpdateMenuItem>
    {
        public UpdateMenuItemValidator(ISiteRules siteRules, 
            IPageRules pageRules, 
            ILanguageRules languageRules, 
            IValidator<MenuItemLocalisation> localisationValidator)
            : base(siteRules, pageRules, languageRules, localisationValidator)
        {
        }
    }
}
