using FluentValidation;
using Weapsy.Domain.Model.Roles.Commands;
using Weapsy.Domain.Model.Roles.Rules;

namespace Weapsy.Domain.Model.Roles.Validators
{
    public class CreateRoleValidator : AbstractValidator<CreateRole>
    {
        private readonly IRoleRules _roleRules;

        public CreateRoleValidator(IRoleRules roleRules)
        {
            _roleRules = roleRules;

            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Id is required.");

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Role name is required.")
                .Length(1, 250).WithMessage("Role name maximum length is 250 characters.")
                .Must(HaveUniqueRoleName).WithMessage("Role already exists.");
        }

        private bool HaveUniqueRoleName(string roleName)
        {
            return _roleRules.IsRoleNameUnique(roleName);
        }
    }
}
