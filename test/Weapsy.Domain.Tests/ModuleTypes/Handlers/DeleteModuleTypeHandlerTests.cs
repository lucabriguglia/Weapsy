using System;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.ModuleTypes;
using Weapsy.Domain.ModuleTypes.Commands;
using Weapsy.Domain.ModuleTypes.Handlers;
using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;

namespace Weapsy.Domain.Tests.ModuleTypes.Handlers
{
    [TestFixture]
    public class DeleteModuleTypeHandlerTests
    {
        [Test]
        public void Should_throw_exception_when_module_type_is_not_found()
        {
            var command = new DeleteModuleType
            {
                Id = Guid.NewGuid()
            };

            var repositoryMock = new Mock<IModuleTypeRepository>();
            repositoryMock.Setup(x => x.GetById(command.Id)).Returns((ModuleType)null);

            var validatorMock = new Mock<IValidator<DeleteModuleType>>();

            var deleteModuleTypeHandler = new DeleteModuleTypeHandler(repositoryMock.Object, validatorMock.Object);

            Assert.Throws<Exception>(() => deleteModuleTypeHandler.Handle(command));
        }

        [Test]
        public void Should_throw_validation_exception_when_validation_fails()
        {
            var command = new DeleteModuleType
            {
                Id = Guid.NewGuid()
            };

            var moduleTypeMock = new Mock<ModuleType>();

            var repositoryMock = new Mock<IModuleTypeRepository>();
            repositoryMock.Setup(x => x.GetById(command.Id)).Returns(moduleTypeMock.Object);

            var validatorMock = new Mock<IValidator<DeleteModuleType>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Id", "Id Error") }));

            var deleteModuleTypeHandler = new DeleteModuleTypeHandler(repositoryMock.Object, validatorMock.Object);

            Assert.Throws<Exception>(() => deleteModuleTypeHandler.Handle(command));
        }

        [Test]
        public void Should_update_module_type()
        {
            var command = new DeleteModuleType
            {
                Id = Guid.NewGuid()
            };

            var moduleTypeMock = new Mock<ModuleType>();

            var repositoryMock = new Mock<IModuleTypeRepository>();
            repositoryMock.Setup(x => x.GetById(command.Id)).Returns(moduleTypeMock.Object);

            var validatorMock = new Mock<IValidator<DeleteModuleType>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var deleteModuleTypeHandler = new DeleteModuleTypeHandler(repositoryMock.Object, validatorMock.Object);

            deleteModuleTypeHandler.Handle(command);

            repositoryMock.Verify(x => x.Update(It.IsAny<ModuleType>()));
        }
    }
}
