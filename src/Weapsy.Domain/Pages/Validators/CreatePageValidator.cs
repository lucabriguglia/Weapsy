using System;
using FluentValidation;
using Weapsy.Domain.Model.Pages.Commands;
using Weapsy.Domain.Model.Pages.Rules;
using Weapsy.Domain.Model.Sites.Rules;
using Weapsy.Domain.Model.Languages.Rules;

namespace Weapsy.Domain.Model.Pages.Validators
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
                .NotEmpty().WithMessage("Id is required.")
                .Must(HaveUniqueId).WithMessage("A page with the same id already exists.");
        }

        private bool HaveUniqueId(Guid id)
        {
            return _pageRules.IsPageIdUnique(id);
        }
    }
}