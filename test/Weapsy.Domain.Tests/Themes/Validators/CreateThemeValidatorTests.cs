using System;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Themes.Commands;
using Weapsy.Domain.Themes.Validators;
using Weapsy.Domain.Themes.Rules;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Tests.Themes.Validators
{
    [TestFixture]
    public class CreateThemeValidatorTests
    {
        [Test]
        public void Should_have_validation_error_when_theme_id_already_exists()
        {
            var command = new CreateTheme
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                Folder = "Folder"
            };

            var themeRulesMock = new Mock<IThemeRules>();
            themeRulesMock.Setup(x => x.IsThemeIdUnique(command.Id)).Returns(false);

            var siteRulesMock = new Mock<ISiteRules>();
            var validator = new CreateThemeValidator(themeRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Id, command);
        }

        [Test]
        public void Should_have_validation_error_when_theme_name_is_empty()
        {
            var command = new CreateTheme
            {
                Id = Guid.NewGuid(),
                Name = string.Empty,
                Description = "Description",
                Folder = "Folder"
            };

            var themeRulesMock = new Mock<IThemeRules>();
            var validator = new CreateThemeValidator(themeRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Test]
        public void Should_have_validation_error_when_theme_name_is_too_long()
        {
            var name = "";
            for (int i = 0; i < 101; i++) name += i;

            var command = new CreateTheme
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = "Description",
                Folder = "Folder"
            };

            var themeRulesMock = new Mock<IThemeRules>();
            var validator = new CreateThemeValidator(themeRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Test]
        public void Should_have_validation_error_when_theme_name_is_not_unique()
        {
            var command = new CreateTheme
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                Folder = "Folder"
            };

            var themeRulesMock = new Mock<IThemeRules>();
            themeRulesMock.Setup(x => x.IsThemeNameUnique(command.Name, Guid.Empty)).Returns(false);

            var validator = new CreateThemeValidator(themeRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Test]
        public void Should_have_validation_error_when_description_is_too_long()
        {
            var description = "";
            for (int i = 0; i < 251; i++) description += i;

            var command = new CreateTheme
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = description,
                Folder = "Folder"
            };

            var themeRulesMock = new Mock<IThemeRules>();
            var validator = new CreateThemeValidator(themeRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Description, command);
        }

        [Test]
        public void Should_have_validation_error_when_folder_is_empty()
        {
            var command = new CreateTheme
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                Folder = string.Empty
            };

            var themeRulesMock = new Mock<IThemeRules>();
            var validator = new CreateThemeValidator(themeRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Folder, command);
        }

        [Test]
        public void Should_have_validation_error_when_folder_is_too_long()
        {
            var folder = "";
            for (int i = 0; i < 251; i++) folder += i;

            var command = new CreateTheme
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                Folder = folder
            };

            var themeRulesMock = new Mock<IThemeRules>();
            var validator = new CreateThemeValidator(themeRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Folder, command);
        }

        [Test]
        public void Should_have_validation_error_when_folder_is_not_valid()
        {
            var command = new CreateTheme
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                Folder = "a@b"
            };

            var themeRulesMock = new Mock<IThemeRules>();
            themeRulesMock.Setup(x => x.IsThemeFolderValid(command.Folder)).Returns(false);

            var validator = new CreateThemeValidator(themeRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Folder, command);
        }

        [Test]
        public void Should_have_validation_error_when_folder_is_not_unique()
        {
            var command = new CreateTheme
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                Folder = "Folder"
            };

            var themeRulesMock = new Mock<IThemeRules>();
            themeRulesMock.Setup(x => x.IsThemeFolderUnique(command.Folder, Guid.Empty)).Returns(false);

            var validator = new CreateThemeValidator(themeRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Folder, command);
        }
    }
}
