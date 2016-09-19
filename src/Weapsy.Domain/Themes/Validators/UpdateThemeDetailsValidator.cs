using Weapsy.Domain.Model.Themes.Commands;
using Weapsy.Domain.Model.Themes.Rules;

namespace Weapsy.Domain.Model.Themes.Validators
{
    public class UpdateThemeDetailsValidator : ThemeDetailsValidator<UpdateThemeDetails>
    {
        public UpdateThemeDetailsValidator(IThemeRules themeRules) : base(themeRules)
        {
        }
    }
}
