using FluentValidation;
using System;
using Weapsy.Domain.Languages.Rules;

namespace Weapsy.Domain.Menus.Validators
{
    public class MenuItemLocalisationValidator : AbstractValidator<MenuItemLocalisation>
    {
        private readonly ILanguageRules _languageRules;

        public MenuItemLocalisationValidator(ILanguageRules languageRules)
        {
            _languageRules = languageRules;

            RuleFor(c => c.LanguageId)
                .NotEmpty().WithMessage("Language is required.")
                .Must(BeAnExistingLanguage).WithMessage("Language does not exist.");

            RuleFor(c => c.Text)
                .Length(1, 100).WithMessage("Text length must be between 1 and 100 characters.")
                .When(c => c.Text != string.Empty);

            RuleFor(c => c.Title)
                .Length(1, 100).WithMessage("Title length must be between 1 and 100 characters.")
                .When(c => c.Title != string.Empty);
        }

        private bool BeAnExistingLanguage(Guid languageId)
        {
            return _languageRules.DoesLanguageExist(languageId);
        }
    }
}
