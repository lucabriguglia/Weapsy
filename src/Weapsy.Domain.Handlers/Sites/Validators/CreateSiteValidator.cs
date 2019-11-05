using FluentValidation;
using System.Threading;
using System.Threading.Tasks;
using Weapsy.Domain.Models.Sites.Commands;
using Weapsy.Domain.Models.Sites.Rules;

namespace Weapsy.Domain.Handlers.Sites.Validators
{
    public class CreateSiteValidator : AbstractValidator<CreateSite>
    {
        private readonly ISiteRules _siteRules;

        public CreateSiteValidator(ISiteRules siteRules)
        {
            _siteRules = siteRules;

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Site name is required.")
                .Length(1, 100).WithMessage("Site name length must be between 1 and 100 characters.")
                .MustAsync(HaveUniqueName).WithMessage($"A site with the same name already exists.");
        }

        private Task<bool> HaveUniqueName(string name, CancellationToken cancellationToken)
        {
            return _siteRules.IsNameUniqueAsync(name);
        }
    }
}
