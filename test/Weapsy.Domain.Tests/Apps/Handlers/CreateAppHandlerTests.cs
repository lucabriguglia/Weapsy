using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Apps;
using Weapsy.Domain.Apps.Commands;
using Weapsy.Domain.Apps.Handlers;

namespace Weapsy.Domain.Tests.Apps.Handlers
{
    [TestFixture]
    public class CreateAppHandlerTests
    {
        [Test]
        public void Should_throw_validation_exception_when_validation_fails()
        {
            var command = new CreateApp
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                Folder = "Folder"
            };

            var appRepositoryMock = new Mock<IAppRepository>();

            var validatorMock = new Mock<IValidator<CreateApp>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Id", "Id Error") }));

            var createAppHandler = new CreateAppHandler(appRepositoryMock.Object, validatorMock.Object);

            Assert.Throws<Exception>(() => createAppHandler.Handle(command));
        }

        [Test]
        public void Should_validate_command_and_save_new_app()
        {
            var command = new CreateApp
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                Folder = "Folder"
            };

            var appRepositoryMock = new Mock<IAppRepository>();
            appRepositoryMock.Setup(x => x.Create(It.IsAny<App>()));

            var validatorMock = new Mock<IValidator<CreateApp>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var createAppHandler = new CreateAppHandler(appRepositoryMock.Object, validatorMock.Object);
            createAppHandler.Handle(command);

            validatorMock.Verify(x => x.Validate(command));
            appRepositoryMock.Verify(x => x.Create(It.IsAny<App>()));
        }
    }
}
