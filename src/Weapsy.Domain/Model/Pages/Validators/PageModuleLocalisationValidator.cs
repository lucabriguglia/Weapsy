using FluentValidation;
using System;
using Weapsy.Domain.Model.Languages.Rules;
using Weapsy.Domain.Model.Pages.Commands;
using Weapsy.Domain.Model.Pages.Rules;

namespace Weapsy.Domain.Model.Pages.Validators
{
    public class PageModuleLocalisationValidator : AbstractValidator<UpdatePageModuleDetails.PageModuleLocalisation>
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
