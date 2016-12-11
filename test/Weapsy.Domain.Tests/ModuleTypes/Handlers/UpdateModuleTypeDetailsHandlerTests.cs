using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.ModuleTypes;
using Weapsy.Domain.ModuleTypes.Commands;
using Weapsy.Domain.ModuleTypes.Handlers;

namespace Weapsy.Domain.Tests.ModuleTypes.Handlers
{
    [TestFixture]
    public class UpdateModuleTypeDetailsHandlerTests
    {
        [Test]
        public void Should_throw_exception_when_module_type_is_not_found()
        {
            var command = new UpdateModuleTypeDetails
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Title = "Title",
                Description = "Description"
            };

            var repositoryMock = new Mock<IModuleTypeRepository>();
            repositoryMock.Setup(x => x.GetById(command.Id)).Returns((ModuleType)null);

            var validatorMock = new Mock<IValidator<UpdateModuleTypeDetails>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var createModuleTypeHandler = new UpdateModuleTypeDetailsHandler(repositoryMock.Object, validatorMock.Object);

            Assert.Throws<Exception>(() => createModuleTypeHandler.Handle(command));
        }

        [Test]
        public void Should_throw_validation_exception_when_validation_fails()
        {
            var command = new UpdateModuleTypeDetails
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Title = "Title",
                Description = "Description"
            };

            var repositoryMock = new Mock<IModuleTypeRepository>();
            repositoryMock.Setup(x => x.GetById(command.Id)).Returns(new ModuleType());

            var validatorMock = new Mock<IValidator<UpdateModuleTypeDetails>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Name", "Name Error") }));

            var createModuleTypeHandler = new UpdateModuleTypeDetailsHandler(repositoryMock.Object, validatorMock.Object);

            Assert.Throws<Exception>(() => createModuleTypeHandler.Handle(command));
        }

        [Test]
        public void Should_validate_command_and_update_module_type()
        {
            var command = new UpdateModuleTypeDetails
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Title = "Title",
                Description = "Description"
            };

            var repositoryMock = new Mock<IModuleTypeRepository>();
            repositoryMock.Setup(x => x.GetById(command.Id)).Returns(new ModuleType());

            var validatorMock = new Mock<IValidator<UpdateModuleTypeDetails>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var createModuleTypeHandler = new UpdateModuleTypeDetailsHandler(repositoryMock.Object, validatorMock.Object);
            createModuleTypeHandler.Handle(command);

            validatorMock.Verify(x => x.Validate(command));
            repositoryMock.Verify(x => x.Update(It.IsAny<ModuleType>()));
        }
    }
}
