using System;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;
using Weapsy.Domain.Languages.Rules;
using Weapsy.Domain.Pages.Rules;
using Weapsy.Domain.Sites.Commands;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Sites.Validators
{
    public class UpdateSiteDetailsValidator : AbstractValidator<UpdateSiteDetails>
    {
        private readonly ISiteRules _siteRules;
        private readonly ILanguageRules _languageRules;
        private readonly IPageRules _pageRules;
        private readonly IValidator<SiteLocalisation> _localisationValidator;

        public UpdateSiteDetailsValidator(ISiteRules siteRules,
            ILanguageRules languageRules,
            IPageRules pageRules,
            IValidator<SiteLocalisation> localisationValidator)
        {
            _siteRules = siteRules;
            _languageRules = languageRules;
            _pageRules = pageRules;
            _localisationValidator = localisationValidator;

            RuleFor(c => c.HomePageId)
                .NotEmpty().WithMessage("HomePageId is required.")
                .Must(BeAnExistingPage).WithMessage("Selected home page does not exist.");

            //RuleFor(c => c.Url)
            //    .NotEmpty().WithMessage("Site url is required.")
            //    .Length(1, 50).WithMessage("Site url length must be between 1 and 200 characters.")
            //    .Must(HaveValidUrl).WithMessage("Site url is not valid. Enter only letters, numbers, underscores and hyphens with no spaces.")
            //    .Must(HaveUniqueUrl).WithMessage("A site with the same url already exists.");

            RuleFor(c => c.Title)
                .Length(1, 250).WithMessage("Head title cannot have more than 250 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Title));

            RuleFor(c => c.MetaDescription)
                .Length(1, 500).WithMessage("Meta description cannot have more than 500 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.MetaDescription));

            RuleFor(c => c.MetaKeywords)
                .Length(1, 500).WithMessage("Meta keywords cannot have more than 500 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.MetaKeywords));

            RuleFor(c => c.SiteLocalisations)
                .Must(IncludeAllSupportedLanguages).WithMessage("All supported languages should be included.");

            RuleFor(c => c.SiteLocalisations)
                .SetCollectionValidator(_localisationValidator);
        }

        private bool BeAnExistingPage(UpdateSiteDetails cmd, Guid pageId)
        {
            return _pageRules.DoesPageExist(cmd.SiteId, pageId);
        }

        //private bool HaveUniqueUrl(UpdateSiteDetails cmd, string url)
        //{
        //    return _siteRules.IsSiteUrlUnique(url, cmd.SiteId);
        //}

        //private bool HaveValidUrl(string url)
        //{
        //    return _siteRules.IsSiteUrlValid(url);
        //}

        private bool IncludeAllSupportedLanguages(UpdateSiteDetails cmd, IEnumerable<SiteLocalisation> siteLocalisations)
        {
            return _languageRules.AreAllSupportedLanguagesIncluded(cmd.SiteId, siteLocalisations.Select(x => x.LanguageId));
        }
    }
}
