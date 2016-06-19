using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Model.Modules;
using Weapsy.Domain.Model.Modules.Commands;
using Weapsy.Domain.Model.Modules.Handlers;

namespace Weapsy.Domain.Tests.Modules.Handlers
{
    [TestFixture]
    public class CreateModuleHandlerTests
    {
        [Test]
        public void Should_throw_validation_exception_when_validation_fails()
        {
            var command = new CreateModule
            {
                SiteId = Guid.NewGuid(),
                ModuleTypeId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Title = "Title"
            };

            var repositoryMock = new Mock<IModuleRepository>();

            var validatorMock = new Mock<IValidator<CreateModule>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Id", "Id Error") }));

            var createModuleHandler = new CreateModuleHandler(repositoryMock.Object, validatorMock.Object);

            Assert.Throws<ValidationException>(() => createModuleHandler.Handle(command));
        }

        [Test]
        public void Should_validate_command_and_save_new_module()
        {
            var command = new CreateModule
            {
                SiteId = Guid.NewGuid(),
                ModuleTypeId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Title = "Title"
            };

            var repositoryMock = new Mock<IModuleRepository>();
            repositoryMock.Setup(x => x.Create(It.IsAny<Module>()));

            var validatorMock = new Mock<IValidator<CreateModule>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var createModuleHandler = new CreateModuleHandler(repositoryMock.Object, validatorMock.Object);
            createModuleHandler.Handle(command);

            validatorMock.Verify(x => x.Validate(command));
            repositoryMock.Verify(x => x.Create(It.IsAny<Module>()));
        }
    }
}
