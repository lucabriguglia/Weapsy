using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Model.Roles;
using Weapsy.Domain.Model.Roles.Commands;
using Weapsy.Domain.Model.Roles.Handlers;

namespace Weapsy.Domain.Tests.Roles.Handlers
{
    [TestFixture]
    public class CreateRoleHandlerTests
    {
        [Test]
        public void Should_throw_validation_exception_when_validation_fails()
        {
            var command = new CreateRole
            {
                Id = Guid.NewGuid(),
                Name = "Name"
            };

            var roleRepositoryMock = new Mock<IRoleRepository>();

            var validatorMock = new Mock<IValidator<CreateRole>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Name", "Name Error") }));

            var createRoleHandler = new CreateRoleHandler(roleRepositoryMock.Object, validatorMock.Object);

            Assert.Throws<ValidationException>(() => createRoleHandler.Handle(command));
        }

        [Test]
        public void Should_validate_command_and_save_new_role()
        {
            var command = new CreateRole
            {
                Id = Guid.NewGuid(),
                Name = "Name"
            };

            var roleRepositoryMock = new Mock<IRoleRepository>();
            roleRepositoryMock.Setup(x => x.Create(It.IsAny<Role>()));

            var validatorMock = new Mock<IValidator<CreateRole>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var createRoleHandler = new CreateRoleHandler(roleRepositoryMock.Object, validatorMock.Object);
            createRoleHandler.Handle(command);

            validatorMock.Verify(x => x.Validate(command));
            roleRepositoryMock.Verify(x => x.Create(It.IsAny<Role>()));
        }
    }
}
