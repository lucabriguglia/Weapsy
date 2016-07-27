using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using Weapsy.Domain.Model.Languages.Rules;
using Weapsy.Domain.Model.Pages.Commands;
using Weapsy.Domain.Model.Pages.Rules;
using Weapsy.Domain.Model.Sites.Rules;

namespace Weapsy.Domain.Model.Pages.Validators
{
    public class PageDetailsValidator<T> : AbstractValidator<T> where T : PageDetails
    {
        private readonly IPageRules _pageRules;
        private readonly ISiteRules _siteRules;
        private readonly ILanguageRules _languageRules;
        private readonly IValidator<PageLocalisation> _localisationValidator;

        public PageDetailsValidator(IPageRules pageRules, 
            ISiteRules siteRules,
            ILanguageRules languageRules,
            IValidator<PageLocalisation> localisationValidator)
        {
            _pageRules = pageRules;
            _siteRules = siteRules;
            _languageRules = languageRules;
            _localisationValidator = localisationValidator;

            RuleFor(c => c.SiteId)
                .NotEmpty().WithMessage("Site id is required.")
                .Must(BeAnExistingSite).WithMessage("Site does not exist.");

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Page name is required.")
                .Length(1, 100).WithMessage("Page name length must be between 1 and 100 characters.")
                .Must(HaveValidName).WithMessage("Page name is not valid. Enter only letters, numbers, underscores and hyphens.")
                .Must(HaveUniqueName).WithMessage("A page with the same name already exists.");

            RuleFor(c => c.Url)
                .NotEmpty().WithMessage("Page url is required.")
                .Length(1, 200).WithMessage("Page url length must be between 1 and 200 characters.")
                .Must(HaveValidUrl).WithMessage("Page url is not valid. Enter only letters, numbers, slashes, underscores and hyphens with no spaces.")
                .Must(NotBeReserved).WithMessage("Page url is reserved.")
                .Must(HaveUniqueUrl).WithMessage("A page with the same url already exists.");

            RuleFor(c => c.Title)
                .Length(1, 250).WithMessage("Head title cannot have more than 250 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Title));

            RuleFor(c => c.MetaDescription)
                .Length(1, 500).WithMessage("Meta description cannot have more than 500 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.MetaDescription));

            RuleFor(c => c.MetaKeywords)
                .Length(1, 500).WithMessage("Meta keywords cannot have more than 500 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.MetaKeywords));

            RuleFor(c => c.PageLocalisations)
                .Must(IncludeAllSupportedLanguages).WithMessage("All supported languages should be included.");

            RuleFor(c => c.PageLocalisations)
                .SetCollectionValidator(_localisationValidator);
        }

        private bool BeAnExistingSite(Guid siteId)
        {
            return _siteRules.DoesSiteExist(siteId);
        }

        private bool HaveUniqueName(PageDetails cmd, string name)
        {
            return _pageRules.IsPageNameUnique(cmd.SiteId, name, cmd.Id);
        }

        private bool HaveValidName(string name)
        {
            return _pageRules.IsPageNameValid(name);
        }

        private bool HaveUniqueUrl(PageDetails cmd, string url)
        {
            return _pageRules.IsPageUrlUnique(cmd.SiteId, url, cmd.Id);
        }

        private bool HaveValidUrl(string url)
        {
            return _pageRules.IsPageUrlValid(url);
        }

        private bool NotBeReserved(string url)
        {
            return !_pageRules.IsPageUrlReserved(url);
        }

        private bool IncludeAllSupportedLanguages(PageDetails cmd, IEnumerable<PageLocalisation> pageLocalisations)
        {
            return _languageRules.AreAllSupportedLanguagesIncluded(cmd.SiteId, pageLocalisations.Select(x => x.LanguageId));
        }
    }
}
