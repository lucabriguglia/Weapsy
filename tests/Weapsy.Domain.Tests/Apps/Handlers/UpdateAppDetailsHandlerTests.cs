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
    public class UpdateAppDetailsHandlerTests
    {
        [Test]
        public void Should_throw_validation_exception_when_validation_fails()
        {
            var command = new UpdateAppDetails
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                Folder = "Folder"
            };

            var repositoryMock = new Mock<IAppRepository>();
            repositoryMock.Setup(x => x.GetById(command.Id)).Returns(new App());

            var validatorMock = new Mock<IValidator<UpdateAppDetails>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Id", "Id Error") }));

            var createAppHandler = new UpdateAppDetailsHandler(repositoryMock.Object, validatorMock.Object);

            Assert.Throws<Exception>(() => createAppHandler.Handle(command));
        }

        [Test]
        public void Should_validate_command_and_update_app()
        {
            var command = new UpdateAppDetails
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                Folder = "Folder"
            };

            var repositoryMock = new Mock<IAppRepository>();
            repositoryMock.Setup(x => x.GetById(command.Id)).Returns(new App());

            var validatorMock = new Mock<IValidator<UpdateAppDetails>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var createAppHandler = new UpdateAppDetailsHandler(repositoryMock.Object, validatorMock.Object);
            createAppHandler.Handle(command);

            validatorMock.Verify(x => x.Validate(command));
            repositoryMock.Verify(x => x.Update(It.IsAny<App>()));
        }
    }
}
