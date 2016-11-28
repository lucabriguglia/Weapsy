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
    public class CreateModuleTypeHandlerTests
    {
        [Test]
        public void Should_throw_validation_exception_when_validation_fails()
        {
            var command = new CreateModuleType
            {
                AppId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My ModuleType",
                Title = "Title",
                Description = "Description"
            };

            var languageRepositoryMock = new Mock<IModuleTypeRepository>();

            var validatorMock = new Mock<IValidator<CreateModuleType>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Id", "Id Error") }));

            var createModuleTypeHandler = new CreateModuleTypeHandler(languageRepositoryMock.Object, validatorMock.Object);

            Assert.Throws<Exception>(() => createModuleTypeHandler.Handle(command));
        }

        [Test]
        public void Should_validate_command_and_save_new_module_Type()
        {
            var command = new CreateModuleType
            {
                AppId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My ModuleType",
                Title = "Title",
                Description = "Description"
            };

            var languageRepositoryMock = new Mock<IModuleTypeRepository>();
            languageRepositoryMock.Setup(x => x.Create(It.IsAny<ModuleType>()));

            var validatorMock = new Mock<IValidator<CreateModuleType>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var createModuleTypeHandler = new CreateModuleTypeHandler(languageRepositoryMock.Object, validatorMock.Object);
            createModuleTypeHandler.Handle(command);

            validatorMock.Verify(x => x.Validate(command));
            languageRepositoryMock.Verify(x => x.Create(It.IsAny<ModuleType>()));
        }
    }
}
