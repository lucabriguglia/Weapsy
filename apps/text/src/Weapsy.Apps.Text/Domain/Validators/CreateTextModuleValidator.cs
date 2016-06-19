using System;
using FluentValidation;
using Weapsy.Apps.Text.Domain.Rules;
using Weapsy.Apps.Text.Domain.Commands;
using Weapsy.Domain.Model.Sites.Rules;
using Weapsy.Domain.Model.Modules.Rules;

namespace Weapsy.Apps.Text.Domain.Validators
{
    public class CreateTextModuleValidator : AbstractValidator<CreateTextModule>
    {
        private readonly ITextModuleRules _textModuleRules;
        private readonly IModuleRules _moduleRules;
        private readonly ISiteRules _siteRules;

        public CreateTextModuleValidator(ITextModuleRules textModuleRules, IModuleRules moduleRules, ISiteRules siteRules)
        {
            _textModuleRules = textModuleRules;
            _moduleRules = moduleRules;
            _siteRules = siteRules;

            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Id is required.")
                .Must(HaveUniqueId).WithMessage("An text with the same id already exists.");

            RuleFor(c => c.ModuleId)
                .NotEmpty().WithMessage("Module id is required.")
                .Must(BeAnExistingModule).WithMessage("Module does not exist.");

            RuleFor(c => c.SiteId)
                .NotEmpty().WithMessage("Site id is required.")
                .Must(BeAnExistingSite).WithMessage("Site does not exist.");

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

        private bool BeAnExistingSite(Guid siteId)
        {
            return _siteRules.DoesSiteExist(siteId);
        }
    }
}
