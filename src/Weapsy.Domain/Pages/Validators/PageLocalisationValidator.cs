using FluentValidation;
using System;
using Weapsy.Domain.Languages.Rules;
using Weapsy.Domain.Pages.Rules;

namespace Weapsy.Domain.Pages.Validators
{
    public class PageLocalisationValidator : AbstractValidator<PageLocalisation>
    {
        private readonly IPageRules _pageRules;
        private readonly ILanguageRules _languageRules;

        public PageLocalisationValidator(IPageRules pageRules, ILanguageRules languageRules)
        {
            _pageRules = pageRules;
            _languageRules = languageRules;

            RuleFor(c => c.LanguageId)
                .NotEmpty().WithMessage("Language is required.")
                .Must(BeAnExistingLanguage).WithMessage("Language does not exist.");

            RuleFor(c => c.Url)
                .Length(1, 200).WithMessage("Page url cannot have more than 200 characters.")
                .Must(HaveValidUrl).WithMessage("Page url is not valid. Enter only letters, numbers, underscores and hyphens with no spaces.")
                .When(x => !string.IsNullOrWhiteSpace(x.Url));

            RuleFor(c => c.Title)
                .Length(1, 250).WithMessage("Head title cannot have more than 250 characters.")
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

        private bool HaveValidUrl(string url)
        {
            return _pageRules.IsPageUrlValid(url);
        }
    }
}
