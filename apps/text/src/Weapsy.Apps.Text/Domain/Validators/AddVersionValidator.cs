using System;
using FluentValidation;
using Weapsy.Apps.Text.Domain.Rules;
using Weapsy.Apps.Text.Domain.Commands;
using Weapsy.Domain.Model.Sites.Rules;
using Weapsy.Domain.Model.Modules.Rules;
using Weapsy.Domain.Model.Languages.Rules;
using System.Collections.Generic;
using System.Linq;

namespace Weapsy.Apps.Text.Domain.Validators
{
    public class AddVersionValidator : AbstractValidator<AddVersion>
    {
        private readonly ITextModuleRules _textModuleRules;
        private readonly IModuleRules _moduleRules;
        private readonly ISiteRules _siteRules;
        private readonly ILanguageRules _languageRules;
        private readonly IValidator<AddVersion.VersionLocalisation> _localisationValidator;

        public AddVersionValidator(ITextModuleRules textModuleRules, 
            IModuleRules moduleRules, 
            ISiteRules siteRules,
            ILanguageRules languageRules,
            IValidator<AddVersion.VersionLocalisation> localisationValidator)
        {
            _textModuleRules = textModuleRules;
            _moduleRules = moduleRules;
            _siteRules = siteRules;
            _languageRules = languageRules;
            _localisationValidator = localisationValidator;

            RuleFor(c => c.ModuleId)
                .NotEmpty().WithMessage("Module id is required.")
                .Must(BeAnExistingModule).WithMessage("Module does not exist.");

            RuleFor(c => c.SiteId)
                .NotEmpty().WithMessage("Site id is required.")
                .Must(BeAnExistingSite).WithMessage("Site does not exist.");

            RuleFor(c => c.Content)
                .NotEmpty().WithMessage("Content is required.");

            RuleFor(c => c.Description)
                .Length(1, 250).WithMessage("Description cannot be longer of 250 characters.")
                .When(c => !string.IsNullOrWhiteSpace(c.Description));

            RuleFor(c => c.VersionLocalisations)
                .Must(IncludeAllSupportedLanguages).WithMessage("All supported languages should be included.");

            RuleFor(c => c.VersionLocalisations)
                .SetCollectionValidator(_localisationValidator);
        }

        private bool HaveUniqueId(Guid id)
        {
            return _textModuleRules.IsTextModuleIdUnique(id);
        }

        private bool BeAnExistingModule(AddVersion cmd, Guid moduleId)
        {
            return _moduleRules.DoesModuleExist(cmd.SiteId, moduleId);
        }

        private bool BeAnExistingSite(Guid siteId)
        {
            return _siteRules.DoesSiteExist(siteId);
        }

        private bool IncludeAllSupportedLanguages(AddVersion cmd, IEnumerable<AddVersion.VersionLocalisation> versionLocalisations)
        {
            return _languageRules.AreAllSupportedLanguagesIncluded(cmd.SiteId, versionLocalisations.Select(x => x.LanguageId));
        }
    }
}
