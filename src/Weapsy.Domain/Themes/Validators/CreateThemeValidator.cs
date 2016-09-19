using System;
using FluentValidation;
using Weapsy.Domain.Themes.Commands;
using Weapsy.Domain.Themes.Rules;

namespace Weapsy.Domain.Themes.Validators
{
    public class CreateThemeValidator : ThemeDetailsValidator<CreateTheme>
    {
        private readonly IThemeRules _themeRules;

        public CreateThemeValidator(IThemeRules themeRules) : base(themeRules)
        {
            _themeRules = themeRules;

            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Id is required.")
                .Must(HaveUniqueId).WithMessage("A theme with the same id already exists.");
        }

        private bool HaveUniqueId(Guid id)
        {
            return _themeRules.IsThemeIdUnique(id);
        }
    }
}
