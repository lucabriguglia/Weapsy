using FluentValidation;
using System;
using Weapsy.Domain.Languages.Rules;

namespace Weapsy.Domain.Sites.Validators
{
    public class SiteLocalisationValidator : AbstractValidator<SiteLocalisation>
    {
        private readonly ILanguageRules _languageRules;

        public SiteLocalisationValidator(ILanguageRules languageRules)
        {
            _languageRules = languageRules;

            RuleFor(c => c.LanguageId)
                .NotEmpty().WithMessage("Language is required.")
                .Must(BeAnExistingLanguage).WithMessage("Language does not exist.");

            RuleFor(c => c.Title)
                .Length(1, 250).WithMessage("Title cannot have more than 250 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Title));

            RuleFor(c => c.MetaDescription)
                .Length(1, 500).WithMessage("Meta description cannot have more than 500 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.MetaDescription));

            RuleFor(c => c.MetaKeywords)
                .Length(1, 500).WithMessage("Meta keywords cannot have more than 500 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.MetaKeywords));
        }

        private bool BeAnExistingLanguage(Guid languageId)
        {
            return _languageRules.DoesLanguageExist(languageId);
        }
    }
}
