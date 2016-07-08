using System;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Model.Roles.Commands;
using Weapsy.Domain.Model.Roles.Validators;
using Weapsy.Domain.Model.Roles.Rules;

namespace Weapsy.Domain.Tests.Roles.Validators
{
    [TestFixture]
    public class CreateRoleValidatorTests
    {
        [Test]
        public void Should_have_error_when_role_id_is_empty()
        {
            var command = new CreateRole
            {
                Id = Guid.Empty,
                Name = "Name"
            };

            var roleRules = new Mock<IRoleRules>();
            var validator = new CreateRoleValidator(roleRules.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Id, command);
        }

        [Test]
        public void Should_have_error_when_role_name_is_empty()
        {
            var command = new CreateRole
            {
                Id = Guid.NewGuid(),
                Name = ""
            };

            var roleRules = new Mock<IRoleRules>();
            var validator = new CreateRoleValidator(roleRules.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Test]
        public void Should_have_error_when_role_name_is_too_long()
        {
            var name = "";
            for (int i = 0; i < 251; i++) name += i;

            var command = new CreateRole
            {
                Id = Guid.NewGuid(),
                Name = name
            };

            var roleRules = new Mock<IRoleRules>();
            var validator = new CreateRoleValidator(roleRules.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Test]
        public void Should_have_error_when_role_name_already_exists()
        {
            var command = new CreateRole
            {
                Id = Guid.NewGuid(),
                Name = "Name"
            };

            var roleRules = new Mock<IRoleRules>();
            roleRules.Setup(x => x.IsRoleNameUnique(command.Name)).Returns(false);

            var validator = new CreateRoleValidator(roleRules.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }
    }
}
