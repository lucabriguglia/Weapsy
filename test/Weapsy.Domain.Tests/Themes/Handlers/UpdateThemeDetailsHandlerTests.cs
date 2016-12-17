using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Themes;
using Weapsy.Domain.Themes.Commands;
using Weapsy.Domain.Themes.Handlers;

namespace Weapsy.Domain.Tests.Themes.Handlers
{
    [TestFixture]
    public class UpdateThemeDetailsHandlerTests
    {
        [Test]
        public void Should_throw_exception_when_theme_is_not_found()
        {
            var command = new UpdateThemeDetails
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                Folder = "Folder"
            };

            var themeRepositoryMock = new Mock<IThemeRepository>();
            themeRepositoryMock.Setup(x => x.GetById(command.Id)).Returns((Theme)null);

            var validatorMock = new Mock<IValidator<UpdateThemeDetails>>();

            var createThemeHandler = new UpdateThemeDetailsHandler(themeRepositoryMock.Object, validatorMock.Object);

            Assert.Throws<Exception>(() => createThemeHandler.Handle(command));
        }

        [Test]
        public void Should_throw_validation_exception_when_validation_fails()
        {
            var command = new UpdateThemeDetails
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                Folder = "Folder"
            };

            var themeRepositoryMock = new Mock<IThemeRepository>();
            themeRepositoryMock.Setup(x => x.GetById(command.Id)).Returns(new Theme());

            var validatorMock = new Mock<IValidator<UpdateThemeDetails>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Name", "Name Error") }));

            var createThemeHandler = new UpdateThemeDetailsHandler(themeRepositoryMock.Object, validatorMock.Object);

            Assert.Throws<Exception>(() => createThemeHandler.Handle(command));
        }

        [Test]
        public void Should_validate_command_and_save_new_theme()
        {
            var command = new UpdateThemeDetails
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                Folder = "Folder"
            };

            var themeRepositoryMock = new Mock<IThemeRepository>();
            themeRepositoryMock.Setup(x => x.GetById(command.Id)).Returns(new Theme());
            themeRepositoryMock.Setup(x => x.Update(It.IsAny<Theme>()));

            var validatorMock = new Mock<IValidator<UpdateThemeDetails>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var createThemeHandler = new UpdateThemeDetailsHandler(themeRepositoryMock.Object, validatorMock.Object);
            createThemeHandler.Handle(command);

            validatorMock.Verify(x => x.Validate(command));
            themeRepositoryMock.Verify(x => x.Update(It.IsAny<Theme>()));
        }
    }
}
