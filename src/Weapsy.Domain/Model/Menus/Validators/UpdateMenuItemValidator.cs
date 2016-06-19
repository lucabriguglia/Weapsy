using FluentValidation;
using Weapsy.Domain.Model.Languages.Rules;
using Weapsy.Domain.Model.Menus.Commands;
using Weapsy.Domain.Model.Pages.Rules;
using Weapsy.Domain.Model.Sites.Rules;

namespace Weapsy.Domain.Model.Menus.Validators
{
    public class UpdateMenuItemValidator : MenuItemValidator<UpdateMenuItem>
    {
        public UpdateMenuItemValidator(ISiteRules siteRules, 
            IPageRules pageRules, 
            ILanguageRules languageRules, 
            IValidator<MenuItemDetails.MenuItemLocalisation> localisationValidator)
            : base(siteRules, pageRules, languageRules, localisationValidator)
        {
        }
    }
}
