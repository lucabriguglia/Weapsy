using System;
using FluentValidation;
using Weapsy.Domain.Templates.Commands;
using Weapsy.Domain.Templates.Rules;
using Weapsy.Domain.Themes.Rules;

namespace Weapsy.Domain.Templates.Validators
{
    public class TemplateDetailsValidator<T> : AbstractValidator<T> where T : TemplateDetails
    {
        private readonly ITemplateRules _templateRules;
        private readonly IThemeRules _themeRules;

        public TemplateDetailsValidator(ITemplateRules templateRules, IThemeRules themeRules)
        {
            _templateRules = templateRules;
            _themeRules = themeRules;

            RuleFor(c => c.ThemeId)
                .NotEmpty().WithMessage("Theme is required.")
                .Must(HaveValidTheme).WithMessage("Theme does not exist.");

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

        private bool HaveValidTheme(Guid themeId)
        {
            return _themeRules.DoesThemeExist(themeId);
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
