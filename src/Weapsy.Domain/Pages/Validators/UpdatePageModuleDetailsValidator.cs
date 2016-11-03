using FluentValidation;
using Weapsy.Domain.Pages.Commands;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Pages.Validators
{
    public class UpdatePageModuleDetailsValidator : BaseSiteValidator<UpdatePageModuleDetails>
    {
        private readonly IValidator<PageModuleLocalisation> _localisationValidator;

        public UpdatePageModuleDetailsValidator(ISiteRules siteRules, 
            IValidator<PageModuleLocalisation> localisationValidator)
            : base(siteRules)
        {
            _localisationValidator = localisationValidator;

            RuleFor(c => c.Title)
                .Length(1, 250).WithMessage("Title cannot have more than 250 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Title));

            RuleFor(c => c.PageModuleLocalisations)
                .SetCollectionValidator(_localisationValidator);
        }
    }
}
