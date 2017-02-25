using System;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Users.Commands;
using Weapsy.Domain.Users.Rules;
using Weapsy.Domain.Users.Validator;

namespace Weapsy.Domain.Tests.Users.Validators
{
    [TestFixture]
    public class CreateUserValidatorTests
    {
        [Test]
        public void Should_have_error_when_user_email_is_empty()
        {
            var command = new CreateUser
            {
                Id = Guid.NewGuid(),
                Email = "",
                UserName = "my"
            };

            var userRules = new Mock<IUserRules>();
            var validator = new CreateUserValidator(userRules.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Email, command);
        }

        [Test]
        public void Should_have_error_when_user_email_is_too_long()
        {
            var email = "";
            for (int i = 0; i < 251; i++) email += i;

            var command = new CreateUser
            {
                Id = Guid.NewGuid(),
                Email = email,
                UserName = "my"
            };

            var userRules = new Mock<IUserRules>();
            var validator = new CreateUserValidator(userRules.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Email, command);
        }

        [Test]
        public void Should_have_error_when_user_email_already_exists()
        {
            var command = new CreateUser
            {
                Id = Guid.NewGuid(),
                Email = "my@email.com",
                UserName = "my"
            };

            var userRules = new Mock<IUserRules>();
            userRules.Setup(x => x.IsUserEmailUnique(command.Email, new Guid())).Returns(false);

            var validator = new CreateUserValidator(userRules.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Email, command);
        }

        [Test]
        public void Should_have_error_when_user_name_is_empty()
        {
            var command = new CreateUser
            {
                Id = Guid.NewGuid(),
                Email = "my@email.com",
                UserName = ""
            };

            var userRules = new Mock<IUserRules>();
            var validator = new CreateUserValidator(userRules.Object);

            validator.ShouldHaveValidationErrorFor(x => x.UserName, command);
        }

        [Test]
        public void Should_have_error_when_user_name_is_too_long()
        {
            var userName = "";
            for (int i = 0; i < 251; i++) userName += i;

            var command = new CreateUser
            {
                Id = Guid.NewGuid(),
                Email = "my@email.com",
                UserName = userName
            };

            var userRules = new Mock<IUserRules>();
            var validator = new CreateUserValidator(userRules.Object);

            validator.ShouldHaveValidationErrorFor(x => x.UserName, command);
        }

        [Test]
        public void Should_have_error_when_user_name_already_exists()
        {
            var command = new CreateUser
            {
                Id = Guid.NewGuid(),
                Email = "my@email.com",
                UserName = "my"
            };

            var userRules = new Mock<IUserRules>();
            userRules.Setup(x => x.IsUserNameUnique(command.UserName, new Guid())).Returns(false);

            var validator = new CreateUserValidator(userRules.Object);

            validator.ShouldHaveValidationErrorFor(x => x.UserName, command);
        }
    }
}
