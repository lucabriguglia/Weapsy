using FluentValidation;
using System.Threading;
using System.Threading.Tasks;
using Weapsy.Domain.Models.Sites.Commands;
using Weapsy.Domain.Models.Sites.Rules;

namespace Weapsy.Domain.Handlers.Sites.Validators
{
    public class CreateSiteValidator : AbstractValidator<CreateSite>
    {
        private readonly ISiteRules _rules;

        public CreateSiteValidator(ISiteRules rules)
        {
            _rules = rules;

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Site name is required.")
                .Length(1, 100).WithMessage("Site name length must be between 1 and 100 characters.")
                .MustAsync(HaveUniqueName).WithMessage($"A site with the same name already exists.");
        }

        private Task<bool> HaveUniqueName(string name, CancellationToken cancellationToken)
        {
            return _rules.IsNameUniqueAsync(name);
        }
    }
}
