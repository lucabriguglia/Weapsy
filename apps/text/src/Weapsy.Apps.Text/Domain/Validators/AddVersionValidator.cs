using System;
using FluentValidation;
using Weapsy.Apps.Text.Domain.Rules;
using Weapsy.Apps.Text.Domain.Commands;
using Weapsy.Domain.Sites.Rules;
using Weapsy.Domain.Modules.Rules;
using Weapsy.Domain.Languages.Rules;
using System.Collections.Generic;
using System.Linq;
using Weapsy.Domain;

namespace Weapsy.Apps.Text.Domain.Validators
{
    public class AddVersionValidator : BaseSiteValidator<AddVersion>
    {
        private readonly ITextModuleRules _textModuleRules;
        private readonly IModuleRules _moduleRules;
        private readonly ILanguageRules _languageRules;
        private readonly IValidator<AddVersion.VersionLocalisation> _localisationValidator;

        public AddVersionValidator(ITextModuleRules textModuleRules, 
            IModuleRules moduleRules, 
            ISiteRules siteRules,
            ILanguageRules languageRules,
            IValidator<AddVersion.VersionLocalisation> localisationValidator)
            : base(siteRules)
        {
            _textModuleRules = textModuleRules;
            _moduleRules = moduleRules;
            _languageRules = languageRules;
            _localisationValidator = localisationValidator;

            RuleFor(c => c.ModuleId)
                .NotEmpty().WithMessage("Module id is required.")
                .Must(BeAnExistingModule).WithMessage("Module does not exist.");

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

        private bool IncludeAllSupportedLanguages(AddVersion cmd, IEnumerable<AddVersion.VersionLocalisation> versionLocalisations)
        {
            return _languageRules.AreAllSupportedLanguagesIncluded(cmd.SiteId, versionLocalisations.Select(x => x.LanguageId));
        }
    }
}
