using FluentValidation;
using Weapsy.Domain.Languages.Commands;
using Weapsy.Domain.Languages.Rules;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Languages.Validators
{
    public class LanguageDetailsValidator<T> : BaseSiteValidator<T> where T : LanguageDetails
    {
        private readonly ILanguageRules _languageRules;

        public LanguageDetailsValidator(ILanguageRules languageRules, ISiteRules siteRules)
            : base(siteRules)
        {
            _languageRules = languageRules;

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Language name is required.")
                .Length(2, 100).WithMessage("Language name length must be between 2 and 100 characters.")
                .Must(HaveValidName).WithMessage("Language name is not valid. Enter only letters, numbers, underscores and hyphens.")
                .Must(HaveUniqueName).WithMessage("A language with the same name already exists.");

            RuleFor(c => c.CultureName)
                .NotEmpty().WithMessage("Culture name is required.")
                .Length(2, 100).WithMessage("Culture name length must be between 2 and 100 characters.")
                .Must(HaveValidCultureName).WithMessage("Culture name is not valid.")
                .Must(HaveUniqueCultureName).WithMessage("A language with the same culture name already exists.");

            RuleFor(c => c.Url)
                .NotEmpty().WithMessage("Language url is required.")
                .Length(2, 100).WithMessage("Language url length must be between 2 and 100 characters.")
                .Must(HaveValidLanguageUrl).WithMessage("Language url is not valid. Enter only letters and hyphens.")
                .Must(HaveUniqueLanguageUrl).WithMessage("A language with the same url already exists.");
        }

        private bool HaveValidName(string name)
        {
            return _languageRules.IsLanguageNameValid(name);
        }

        private bool HaveUniqueName(LanguageDetails cmd, string name)
        {
            return _languageRules.IsLanguageNameUnique(cmd.SiteId, name, cmd.Id);
        }

        private bool HaveValidCultureName(string cultureName)
        {
            return _languageRules.IsCultureNameValid(cultureName);
        }

        private bool HaveUniqueCultureName(LanguageDetails cmd, string cultureName)
        {
            return _languageRules.IsCultureNameUnique(cmd.SiteId, cultureName, cmd.Id);
        }

        private bool HaveValidLanguageUrl(string url)
        {
            return _languageRules.IsLanguageUrlValid(url);
        }

        private bool HaveUniqueLanguageUrl(LanguageDetails cmd, string url)
        {
            return _languageRules.IsLanguageUrlUnique(cmd.SiteId, url, cmd.Id);
        }
    }
}
