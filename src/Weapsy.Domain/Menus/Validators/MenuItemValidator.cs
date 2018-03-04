using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using Weapsy.Domain.Languages.Rules;
using Weapsy.Domain.Menus.Commands;
using Weapsy.Domain.Pages.Rules;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Menus.Validators
{
    public class MenuItemValidator<T> : BaseSiteValidator<T> where T : MenuItemDetails
    {
        private readonly IPageRules _pageRules;
        private readonly ILanguageRules _languageRules;
        private readonly IValidator<MenuItemLocalisation> _localisationValidator;

        public MenuItemValidator(ISiteRules siteRules, 
            IPageRules pageRules, 
            ILanguageRules languageRules, 
            IValidator<MenuItemLocalisation> localisationValidator)
            : base(siteRules)
        {
            _pageRules = pageRules;
            _languageRules = languageRules;
            _localisationValidator = localisationValidator;

            RuleFor(c => c.PageId)
                .NotEmpty().WithMessage("Page is required.")
                .Must(BeAnExistingPage).WithMessage("Page does not exist.")
                .When(c => c.Type == MenuItemType.Page);

            RuleFor(c => c.Link)
                .NotEmpty().WithMessage("Link is required.")
                .Length(1, 250).WithMessage("Link length must be between 1 and 250 characters.")
                .When(c => c.Type == MenuItemType.Link);

            RuleFor(c => c.Text)
                .NotEmpty().WithMessage("Text is required.")
                .Length(1, 100).WithMessage("Text length must be between 1 and 100 characters.");

            RuleFor(c => c.Title)
                .Length(1, 100).WithMessage("Title length must be between 1 and 100 characters.")
                .When(c => c.Title != string.Empty);

            RuleFor(c => c.MenuItemLocalisations)
                .Must(IncludeAllSupportedLanguages).WithMessage("All supported languages should be included.");

            RuleFor(c => c.MenuItemLocalisations)
                .SetCollectionValidator(_localisationValidator);
        }

        private bool BeAnExistingPage(MenuItemDetails cmd, Guid pageId)
        {
            return _pageRules.DoesPageExist(cmd.SiteId, pageId);
        }

        private bool IncludeAllSupportedLanguages(MenuItemDetails cmd, IEnumerable<MenuItemLocalisation> localisations)
        {
            return _languageRules.AreAllSupportedLanguagesIncluded(cmd.SiteId, localisations.Select(x => x.LanguageId));
        }
    }
}
