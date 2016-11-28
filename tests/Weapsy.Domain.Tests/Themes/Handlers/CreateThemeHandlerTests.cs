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
    public class CreateThemeHandlerTests
    {
        [Test]
        public void Should_throw_validation_exception_when_validation_fails()
        {
            var command = new CreateTheme
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                Folder = "Folder"
            };

            var themeRepositoryMock = new Mock<IThemeRepository>();

            var validatorMock = new Mock<IValidator<CreateTheme>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Id", "Id Error") }));

            var sortOrderGeneratorMock = new Mock<IThemeSortOrderGenerator>();

            var createThemeHandler = new CreateThemeHandler(themeRepositoryMock.Object, validatorMock.Object, sortOrderGeneratorMock.Object);

            Assert.Throws<Exception>(() => createThemeHandler.Handle(command));
        }

        [Test]
        public void Should_validate_command_and_save_new_theme()
        {
            var command = new CreateTheme
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                Folder = "Folder"
            };

            var themeRepositoryMock = new Mock<IThemeRepository>();
            themeRepositoryMock.Setup(x => x.Create(It.IsAny<Theme>()));

            var validatorMock = new Mock<IValidator<CreateTheme>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var sortOrderGeneratorMock = new Mock<IThemeSortOrderGenerator>();

            var createThemeHandler = new CreateThemeHandler(themeRepositoryMock.Object, validatorMock.Object, sortOrderGeneratorMock.Object);
            createThemeHandler.Handle(command);

            validatorMock.Verify(x => x.Validate(command));
            themeRepositoryMock.Verify(x => x.Create(It.IsAny<Theme>()));
        }
    }
}
