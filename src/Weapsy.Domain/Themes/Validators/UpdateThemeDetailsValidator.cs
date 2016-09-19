using Weapsy.Domain.Themes.Commands;
using Weapsy.Domain.Themes.Rules;

namespace Weapsy.Domain.Themes.Validators
{
    public class UpdateThemeDetailsValidator : ThemeDetailsValidator<UpdateThemeDetails>
    {
        public UpdateThemeDetailsValidator(IThemeRules themeRules) : base(themeRules)
        {
        }
    }
}
