using System;
using FluentValidation;
using Weapsy.Domain.Templates.Commands;
using Weapsy.Domain.Templates.Rules;
using Weapsy.Domain.Themes.Rules;

namespace Weapsy.Domain.Templates.Validators
{
    public class CreateTemplateValidator : TemplateDetailsValidator<CreateTemplate>
    {
        private readonly ITemplateRules _templateRules;

        public CreateTemplateValidator(ITemplateRules templateRules, IThemeRules themeRules) 
            : base(templateRules, themeRules)
        {
            _templateRules = templateRules;

            RuleFor(c => c.Id)
                .Must(HaveUniqueId).WithMessage("A template with the same id already exists.")
                .When(x => x.Id != Guid.Empty);
        }

        private bool HaveUniqueId(Guid id)
        {
            return _templateRules.IsTemplateIdUnique(id);
        }
    }
}
