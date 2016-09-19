using System;
using FluentValidation;
using Weapsy.Domain.Templates.Commands;
using Weapsy.Domain.Templates.Rules;

namespace Weapsy.Domain.Templates.Validators
{
    public class CreateTemplateValidator : TemplateDetailsValidator<CreateTemplate>
    {
        private readonly ITemplateRules _templateRules;

        public CreateTemplateValidator(ITemplateRules templateRules) : base(templateRules)
        {
            _templateRules = templateRules;

            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Id is required.")
                .Must(HaveUniqueId).WithMessage("A template with the same id already exists.");
        }

        private bool HaveUniqueId(Guid id)
        {
            return _templateRules.IsTemplateIdUnique(id);
        }
    }
}
