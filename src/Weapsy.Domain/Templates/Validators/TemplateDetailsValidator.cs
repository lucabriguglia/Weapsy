using FluentValidation;
using Weapsy.Domain.Templates.Commands;
using Weapsy.Domain.Templates.Rules;

namespace Weapsy.Domain.Templates.Validators
{
    public class TemplateDetailsValidator<T> : AbstractValidator<T> where T : TemplateDetails
    {
        private readonly ITemplateRules _templateRules;

        public TemplateDetailsValidator(ITemplateRules templateRules)
        {
            _templateRules = templateRules;

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Template name is required.")
                .Length(1, 100).WithMessage("Template name length must be between 1 and 100 characters.")
                .Must(HaveUniqueName).WithMessage("A template with the same name already exists.");

            RuleFor(c => c.Description)
                .Length(1, 250).WithMessage("Culture name length must be between 1 and 250 characters.")
                .When(c => !string.IsNullOrWhiteSpace(c.Description));

            RuleFor(c => c.ViewName)
                .NotEmpty().WithMessage("Template view name is required.")
                .Length(1, 250).WithMessage("Template view name length must be between 1 and 250 characters.")
                .Must(HaveValidTemplateViewName).WithMessage("Template url is not valid. Enter only letters and 1 hyphen.")
                .Must(HaveUniqueTemplateViewName).WithMessage("A template with the same url already exists.");
        }

        private bool HaveUniqueName(TemplateDetails cmd, string name)
        {
            return _templateRules.IsTemplateNameUnique(name, cmd.Id);
        }

        private bool HaveValidTemplateViewName(string viewName)
        {
            return _templateRules.IsTemplateViewNameValid(viewName);
        }

        private bool HaveUniqueTemplateViewName(string viewName)
        {
            return _templateRules.IsTemplateViewNameUnique(viewName);
        }
    }
}
