using System;
using FluentValidation;
using Weapsy.Domain.Model.Users.Commands;

namespace Weapsy.Domain.Model.Users.Validator
{
    public class CreateUserValidator : AbstractValidator<CreateUser>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Id is required.")
                .Must(HaveUniqueId).WithMessage("A user with the same id already exists.");
        }

        private bool HaveUniqueId(Guid id)
        {
            return _userRepository.GetById(id) == null;
        }
    }
}
