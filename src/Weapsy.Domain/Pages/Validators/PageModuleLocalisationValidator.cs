using FluentValidation;
using System;
using Weapsy.Domain.Languages.Rules;
using Weapsy.Domain.Pages.Rules;

namespace Weapsy.Domain.Pages.Validators
{
    public class PageModuleLocalisationValidator : AbstractValidator<PageModuleLocalisation>
    {
        private readonly IPageRules _pageRules;
        private readonly ILanguageRules _languageRules;

        public PageModuleLocalisationValidator(IPageRules pageRules, ILanguageRules languageRules)
        {
            _pageRules = pageRules;
            _languageRules = languageRules;

            RuleFor(c => c.LanguageId)
                .NotEmpty().WithMessage("Language is required.")
                .Must(BeAnExistingLanguage).WithMessage("Language does not exist.");

            RuleFor(c => c.Title)
                .Length(1, 250).WithMessage("Title cannot have more than 250 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Title));
        }

        private bool BeAnExistingLanguage(Guid languageId)
        {
            return _languageRules.DoesLanguageExist(languageId);
        }
    }
}
