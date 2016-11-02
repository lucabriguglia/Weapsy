using System;
using FluentValidation;
using Weapsy.Domain.Pages.Commands;
using Weapsy.Domain.Pages.Rules;
using Weapsy.Domain.Sites.Rules;
using Weapsy.Domain.Languages.Rules;

namespace Weapsy.Domain.Pages.Validators
{
    public class CreatePageValidator : PageDetailsValidator<CreatePage>
    {
        private readonly IPageRules _pageRules;

        public CreatePageValidator(IPageRules pageRules, 
            ISiteRules siteRules, 
            ILanguageRules languageRules, 
            IValidator<PageLocalisation> localisationValidator)
            : base(pageRules, siteRules, languageRules, localisationValidator)
        {
            _pageRules = pageRules;

            RuleFor(c => c.Id)
                .Must(HaveUniqueId).WithMessage("A page with the same id already exists.")
                .When(x => x.Id != Guid.Empty);
        }

        private bool HaveUniqueId(Guid id)
        {
            return _pageRules.IsPageIdUnique(id);
        }
    }
}