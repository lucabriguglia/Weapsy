using Weapsy.Domain.Templates.Commands;
using Weapsy.Domain.Templates.Rules;
using Weapsy.Domain.Themes.Rules;

namespace Weapsy.Domain.Templates.Validators
{
    public class UpdateTemplateDetailsValidator : TemplateDetailsValidator<UpdateTemplateDetails>
    {
        public UpdateTemplateDetailsValidator(ITemplateRules templateRules, IThemeRules themeRules) 
            : base(templateRules, themeRules)
        {
        }
    }
}
