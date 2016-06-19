using Weapsy.Domain.Model.Templates.Commands;
using Weapsy.Domain.Model.Templates.Rules;

namespace Weapsy.Domain.Model.Templates.Validators
{
    public class UpdateTemplateDetailsValidator : TemplateDetailsValidator<UpdateTemplateDetails>
    {
        public UpdateTemplateDetailsValidator(ITemplateRules templateRules) : base(templateRules)
        {
        }
    }
}
