using System;
using FluentValidation;
using Weapsy.Domain.Sites.Commands;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Sites.Validators
{
    public class CreateSiteValidator : AbstractValidator<CreateSite>
    {
        private readonly ISiteRules _siteRules;

        public CreateSiteValidator(ISiteRules siteRules)
        {
            _siteRules = siteRules;

            RuleFor(c => c.Id)
                .Must(HaveUniqueId).WithMessage("A site with the same id already exists.")
                .When(x => x.Id != Guid.Empty);

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(3, 100).WithMessage("Site name length must be between 3 and 100 characters.")
                .Must(HaveValidName).WithMessage("Site name is not valid. Enter only letters, numbers, underscores and hyphens.")
                .Must(HaveUniqueName).WithMessage("A site with the same name already exists.");
        }

        private bool HaveUniqueId(Guid id)
        {
            return _siteRules.IsSiteIdUnique(id);
        }

        private bool HaveValidName(string name)
        {
            return _siteRules.IsSiteNameValid(name);
        }

        private bool HaveUniqueName(string name)
        {
            return _siteRules.IsSiteNameUnique(name);
        }
    }
}
