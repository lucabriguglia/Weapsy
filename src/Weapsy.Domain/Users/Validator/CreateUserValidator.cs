using System;
using FluentValidation;
using Weapsy.Domain.Users.Commands;
using Weapsy.Domain.Users.Rules;

namespace Weapsy.Domain.Users.Validator
{
    public class CreateUserValidator : AbstractValidator<CreateUser>
    {
        private readonly IUserRules _userRules;

        public CreateUserValidator(IUserRules userRules)
        {
            _userRules = userRules;

            RuleFor(c => c.Id)
                .Must(HaveUniqueId).WithMessage("A user with the same id already exists.")
                .When(x => x.Id != Guid.Empty);

            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email not valid.")
                .Length(1, 250).WithMessage("Email maximum length is 250 characters.")
                .Must(HaveUniqueEmail).WithMessage("Email already exists.");

            RuleFor(c => c.UserName)
                .NotEmpty().WithMessage("UserName is required.")
                .Length(1, 250).WithMessage("UserName maximum length is 250 characters.")
                .Must(HaveUniqueUserName).WithMessage("UserName already exists.");
        }

        private bool HaveUniqueId(Guid id)
        {
            return _userRules.IsUserIdUnique(id);
        }

        private bool HaveUniqueEmail(string email)
        {
            return _userRules.IsUserEmailUnique(email);
        }

        private bool HaveUniqueUserName(string userName)
        {
            return _userRules.IsUserNameUnique(userName);
        }
    }
}
