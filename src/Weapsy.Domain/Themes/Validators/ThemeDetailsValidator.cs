using FluentValidation;
using Weapsy.Domain.Themes.Commands;
using Weapsy.Domain.Themes.Rules;

namespace Weapsy.Domain.Themes.Validators
{
    public class ThemeDetailsValidator<T> : AbstractValidator<T> where T : ThemeDetails
    {
        private readonly IThemeRules _themeRules;

        public ThemeDetailsValidator(IThemeRules themeRules)
        {
            _themeRules = themeRules;

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Theme name is required.")
                .Length(1, 100).WithMessage("Theme name length must be between 1 and 100 characters.")
                .Must(HaveUniqueName).WithMessage("A theme with the same name already exists.");

            RuleFor(c => c.Description)
                .Length(1, 250).WithMessage("Culture name length must be between 1 and 250 characters.")
                .When(c => !string.IsNullOrWhiteSpace(c.Description));

            RuleFor(c => c.Folder)
                .NotEmpty().WithMessage("Theme folder is required.")
                .Length(1, 100).WithMessage("Theme url length must be between 1 and 100 characters.")
                .Must(HaveValidThemeFolder).WithMessage("Theme folder is not valid. Enter only letters and 1 hyphen.")
                .Must(HaveUniqueThemeFolder).WithMessage("A theme with the same folder already exists.");
        }

        private bool HaveUniqueName(string name)
        {
            return _themeRules.IsThemeNameUnique(name);
        }

        private bool HaveValidThemeFolder(string folder)
        {
            return _themeRules.IsThemeFolderValid(folder);
        }

        private bool HaveUniqueThemeFolder(string folder)
        {
            return _themeRules.IsThemeFolderUnique(folder);
        }
    }
}
