using System;
using FluentValidation;
using Weapsy.Domain.Pages.Commands;
using Weapsy.Domain.Modules.Rules;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Pages.Validators
{
    public class AddPageModuleValidator : AbstractValidator<AddPageModule>
    {
        private readonly ISiteRules _siteRules;
        private readonly IModuleRules _moduleRules;

        public AddPageModuleValidator(ISiteRules siteRules, IModuleRules moduleRules)
        {
            _siteRules = siteRules;
            _moduleRules = moduleRules;

            RuleFor(c => c.SiteId)
                .NotEmpty().WithMessage("Site id is required.")
                .Must(BeAnExistingSite).WithMessage("Site does not exist.");

            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Page module id is required.");

            RuleFor(c => c.ModuleId)
                .NotEmpty().WithMessage("Module id is required.")
                .Must(BeAnExistingModule).WithMessage("Module does not exist.");

            RuleFor(c => c.Title)
                .Length(1, 100).WithMessage("Title cannot have more than 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Title));

            RuleFor(c => c.Zone)
                .NotEmpty().WithMessage("Zone is required.")
                .Length(1, 100).WithMessage("Zone cannot have more than 100 characters.");
        }

        private bool BeAnExistingSite(Guid siteId)
        {
            return _siteRules.DoesSiteExist(siteId);
        }

        private bool BeAnExistingModule(AddPageModule cmd, Guid moduleId)
        {
            return _moduleRules.DoesModuleExist(cmd.SiteId, moduleId);
        }
    }
}
