using System;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Themes.Commands;
using Weapsy.Domain.Themes.Validators;
using Weapsy.Domain.Themes.Rules;

namespace Weapsy.Domain.Tests.Themes.Validators
{
    [TestFixture]
    public class ThemeDetailsValidatorTests
    {
        [Test]
        public void Should_have_validation_error_when_theme_name_is_empty()
        {
            var command = new ThemeDetails
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                Folder = "folder"
            };

            var themeRulesMock = new Mock<IThemeRules>();
            var validator = new ThemeDetailsValidator<ThemeDetails>(themeRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Test]
        public void Should_have_validation_error_when_theme_name_is_too_long()
        {
            var name = "";
            for (int i = 0; i < 101; i++) name += i;

            var command = new ThemeDetails
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = "Description",
                Folder = "folder"
            };

            var themeRulesMock = new Mock<IThemeRules>();
            var validator = new ThemeDetailsValidator<ThemeDetails>(themeRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Test]
        public void Should_have_validation_error_when_theme_name_is_not_unique()
        {
            var command = new ThemeDetails
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                Folder = "folder"
            };

            var themeRulesMock = new Mock<IThemeRules>();
            themeRulesMock.Setup(x => x.IsThemeNameUnique(command.Name, Guid.Empty)).Returns(false);

            var validator = new ThemeDetailsValidator<ThemeDetails>(themeRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Test]
        public void Should_have_validation_error_when_description_is_too_long()
        {
            var description = "";
            for (int i = 0; i < 251; i++) description += i;

            var command = new ThemeDetails
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = description,
                Folder = "folder"
            };

            var themeRulesMock = new Mock<IThemeRules>();
            var validator = new ThemeDetailsValidator<ThemeDetails>(themeRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Description, command);
        }

        [Test]
        public void Should_have_validation_error_when_folder_is_empty()
        {
            var command = new ThemeDetails
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                Folder = string.Empty
            };

            var themeRulesMock = new Mock<IThemeRules>();
            var validator = new ThemeDetailsValidator<ThemeDetails>(themeRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Folder, command);
        }

        [Test]
        public void Should_have_validation_error_when_folder_is_too_long()
        {
            var folder = "";
            for (int i = 0; i < 251; i++) folder += i;

            var command = new ThemeDetails
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                Folder = folder
            };

            var themeRulesMock = new Mock<IThemeRules>();
            var validator = new ThemeDetailsValidator<ThemeDetails>(themeRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Folder, command);
        }

        [Test]
        public void Should_have_validation_error_when_folder_is_not_valid()
        {
            var command = new ThemeDetails
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                Folder = "a@b"
            };

            var themeRulesMock = new Mock<IThemeRules>();
            themeRulesMock.Setup(x => x.IsThemeFolderValid(command.Description)).Returns(false);

            var validator = new ThemeDetailsValidator<ThemeDetails>(themeRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Folder, command);
        }

        [Test]
        public void Should_have_validation_error_when_folder_is_not_unique()
        {
            var command = new ThemeDetails
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                Folder = "folder"
            };

            var themeRulesMock = new Mock<IThemeRules>();
            themeRulesMock.Setup(x => x.IsThemeFolderUnique(command.Description, Guid.Empty)).Returns(false);

            var validator = new ThemeDetailsValidator<ThemeDetails>(themeRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Folder, command);
        }
    }
}
