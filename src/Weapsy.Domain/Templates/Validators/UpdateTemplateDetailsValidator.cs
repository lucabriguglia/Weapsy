using Weapsy.Domain.Templates.Commands;
using Weapsy.Domain.Templates.Rules;

namespace Weapsy.Domain.Templates.Validators
{
    public class UpdateTemplateDetailsValidator : TemplateDetailsValidator<UpdateTemplateDetails>
    {
        public UpdateTemplateDetailsValidator(ITemplateRules templateRules) : base(templateRules)
        {
        }
    }
}
