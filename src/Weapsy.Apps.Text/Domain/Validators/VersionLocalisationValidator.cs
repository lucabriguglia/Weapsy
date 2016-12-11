using FluentValidation;
using System;
using Weapsy.Apps.Text.Domain.Commands;
using Weapsy.Domain.Languages.Rules;

namespace Weapsy.Apps.Text.Domain.Validators
{
    public class VersionLocalisationValidator : AbstractValidator<AddVersion.VersionLocalisation>
    {
        private readonly ILanguageRules _languageRules;

        public VersionLocalisationValidator(ILanguageRules languageRules)
        {
            _languageRules = languageRules;

            RuleFor(c => c.LanguageId)
                .NotEmpty().WithMessage("Language is required.")
                .Must(BeAnExistingLanguage).WithMessage("Language does not exist.");

            RuleFor(c => c.Content)
                .Length(1, 251).WithMessage("Content cannot have more than 250 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Content));
        }

        private bool BeAnExistingLanguage(Guid languageId)
        {
            return _languageRules.DoesLanguageExist(languageId);
        }
    }
}
