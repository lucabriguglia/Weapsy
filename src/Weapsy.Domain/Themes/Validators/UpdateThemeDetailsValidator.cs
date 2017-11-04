using Weapsy.Domain.Themes.Commands;
using Weapsy.Domain.Themes.Rules;

namespace Weapsy.Domain.Themes.Validators
{
    public class UpdateThemeDetailsValidator : ThemeDetailsValidator<UpdateThemeDetailsCommand>
    {
        public UpdateThemeDetailsValidator(IThemeRules themeRules) : base(themeRules)
        {
        }
    }
}
