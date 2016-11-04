using System;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Menus.Commands;
using Weapsy.Domain.Menus.Validators;
using Weapsy.Domain.Menus.Rules;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Tests.Menus.Validators
{
    [TestFixture]
    public class CreateMenuValidatorTests
    {
        [Test]
        public void Should_have_validation_error_when_menu_id_is_empty()
        {
            var command = new CreateMenu
            {
                SiteId = Guid.Empty,
                Id = Guid.NewGuid(),
                Name = "My Menu"
            };

            var menuRulesMock = new Mock<IMenuRules>();
            var siteRulesMock = new Mock<ISiteRules>();

            var validator = new CreateMenuValidator(menuRulesMock.Object, siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Id, command);
        }

        [Test]
        public void Should_have_validation_error_when_menu_id_already_exists()
        {
            var command = new CreateMenu
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My Menu"
            };

            var menuRulesMock = new Mock<IMenuRules>();
            menuRulesMock.Setup(x => x.IsMenuIdUnique(command.Id)).Returns(false);

            var siteRulesMock = new Mock<ISiteRules>();

            var validator = new CreateMenuValidator(menuRulesMock.Object, siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Id, command);
        }

        [Test]
        public void Should_have_validation_error_when_menu_name_is_empty()
        {
            var command = new CreateMenu
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = ""
            };

            var menuRulesMock = new Mock<IMenuRules>();
            var siteRulesMock = new Mock<ISiteRules>();

            var validator = new CreateMenuValidator(menuRulesMock.Object, siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Test]
        public void Should_have_validation_error_when_menu_name_is_too_short()
        {
            var command = new CreateMenu
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My"
            };

            var menuRulesMock = new Mock<IMenuRules>();
            var siteRulesMock = new Mock<ISiteRules>();

            var validator = new CreateMenuValidator(menuRulesMock.Object, siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Test]
        public void Should_have_validation_error_when_menu_name_is_too_long()
        {
            var name = "";
            for (int i = 0; i < 101; i++) name += i;

            var command = new CreateMenu
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = name
            };

            var menuRulesMock = new Mock<IMenuRules>();
            var siteRulesMock = new Mock<ISiteRules>();

            var validator = new CreateMenuValidator(menuRulesMock.Object, siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Test]
        public void Should_have_validation_error_when_menu_name_is_not_valid()
        {
            var command = new CreateMenu
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My@Menu"
            };

            var menuRulesMock = new Mock<IMenuRules>();
            menuRulesMock.Setup(x => x.IsMenuNameValid(command.Name)).Returns(false);

            var siteRulesMock = new Mock<ISiteRules>();

            var validator = new CreateMenuValidator(menuRulesMock.Object, siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Test]
        public void Should_have_validation_error_when_menu_name_is_not_unique()
        {
            var command = new CreateMenu
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My Menu"
            };

            var menuRulesMock = new Mock<IMenuRules>();
            menuRulesMock.Setup(x => x.IsMenuNameUnique(command.SiteId, command.Name)).Returns(false);

            var siteRulesMock = new Mock<ISiteRules>();

            var validator = new CreateMenuValidator(menuRulesMock.Object, siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }
    }
}
