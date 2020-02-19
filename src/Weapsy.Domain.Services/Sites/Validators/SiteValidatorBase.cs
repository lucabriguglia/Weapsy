using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Weapsy.Domain.Services.Sites.Rules;

namespace Weapsy.Domain.Services.Sites.Validators
{
    public abstract class SiteValidatorBase<T> : AbstractValidator<T> where T : SiteCommandBase
    {
        private readonly ISiteRules _rules;

        protected SiteValidatorBase(ISiteRules rules)
        {
            _rules = rules;
        }

        protected void ValidateName()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Site name is required.")
                .Length(1, 100).WithMessage(c => $"Site name length must be between 1 and 100 characters. Current length is {c.Name.Length}.")
                .MustAsync(HaveUniqueName).WithMessage(c => $"A site with the name \"{c.Name}\" already exists.");
        }

        private Task<bool> HaveUniqueName(SiteCommandBase command, string name, CancellationToken cancellationToken)
        {
            return _rules.IsNameUniqueAsync(name, command.Id);
        }
    }
}