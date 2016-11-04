using System;
using FluentValidation;
using Weapsy.Apps.Text.Domain.Rules;
using Weapsy.Apps.Text.Domain.Commands;
using Weapsy.Domain;
using Weapsy.Domain.Sites.Rules;
using Weapsy.Domain.Modules.Rules;

namespace Weapsy.Apps.Text.Domain.Validators
{
    public class CreateTextModuleValidator : BaseSiteValidator<CreateTextModule>
    {
        private readonly ITextModuleRules _textModuleRules;
        private readonly IModuleRules _moduleRules;

        public CreateTextModuleValidator(ITextModuleRules textModuleRules, 
            IModuleRules moduleRules, 
            ISiteRules siteRules)
            : base(siteRules)
        {
            _textModuleRules = textModuleRules;
            _moduleRules = moduleRules;

            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Id is required.")
                .Must(HaveUniqueId).WithMessage("An text with the same id already exists.");

            RuleFor(c => c.ModuleId)
                .NotEmpty().WithMessage("Module id is required.")
                .Must(BeAnExistingModule).WithMessage("Module does not exist.");

            RuleFor(c => c.Content)
                .NotEmpty().WithMessage("Content is required.");
        }

        private bool HaveUniqueId(Guid id)
        {
            return _textModuleRules.IsTextModuleIdUnique(id);
        }

        private bool BeAnExistingModule(CreateTextModule cmd, Guid moduleId)
        {
            return _moduleRules.DoesModuleExist(cmd.SiteId, moduleId);
        }
    }
}
