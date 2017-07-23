using System;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Templates.Commands;
using Weapsy.Domain.Templates.Validators;
using Weapsy.Domain.Templates.Rules;
using Weapsy.Domain.Themes.Rules;

namespace Weapsy.Domain.Tests.Templates.Validators
{
    [TestFixture]
    public class TemplateDetailsValidatorTests
    {
        [Test]
        public void Should_have_validation_error_when_theme_does_not_exist()
        {
            var themeId = Guid.NewGuid();

            var command = new TemplateDetails
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                ViewName = "viewName",
                ThemeId = themeId
            };

            var templateRulesMock = new Mock<ITemplateRules>();

            var themeRulesMock = new Mock<IThemeRules>();
            themeRulesMock.Setup(r => r.DoesThemeExist(themeId)).Returns(false);

            var validator = new TemplateDetailsValidator<TemplateDetails>(templateRulesMock.Object, themeRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.ThemeId, command);
        }

        [Test]
        public void Should_have_validation_error_when_template_name_is_empty()
        {
            var command = new TemplateDetails
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                ViewName = "viewName"
            };

            var templateRulesMock = new Mock<ITemplateRules>();
            var themeRulesMock = new Mock<IThemeRules>();
            var validator = new TemplateDetailsValidator<TemplateDetails>(templateRulesMock.Object, themeRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Test]
        public void Should_have_validation_error_when_template_name_is_too_long()
        {
            var name = "";
            for (var i = 0; i < 101; i++) name += i;

            var command = new TemplateDetails
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = "Description",
                ViewName = "viewName"
            };

            var templateRulesMock = new Mock<ITemplateRules>();
            var themeRulesMock = new Mock<IThemeRules>();
            var validator = new TemplateDetailsValidator<TemplateDetails>(templateRulesMock.Object, themeRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Test]
        public void Should_have_validation_error_when_template_name_is_not_unique()
        {
            var command = new TemplateDetails
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                ViewName = "viewName"
            };

            var templateRulesMock = new Mock<ITemplateRules>();
            templateRulesMock.Setup(x => x.IsTemplateNameUnique(command.Name, Guid.Empty)).Returns(false);

            var themeRulesMock = new Mock<IThemeRules>();

            var validator = new TemplateDetailsValidator<TemplateDetails>(templateRulesMock.Object, themeRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Test]
        public void Should_have_validation_error_when_description_is_too_long()
        {
            var description = "";
            for (int i = 0; i < 251; i++) description += i;

            var command = new TemplateDetails
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = description,
                ViewName = "viewName"
            };

            var templateRulesMock = new Mock<ITemplateRules>();
            var themeRulesMock = new Mock<IThemeRules>();
            var validator = new TemplateDetailsValidator<TemplateDetails>(templateRulesMock.Object, themeRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Description, command);
        }

        [Test]
        public void Should_have_validation_error_when_view_name_is_empty()
        {
            var command = new TemplateDetails
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                ViewName = string.Empty
            };

            var templateRulesMock = new Mock<ITemplateRules>();
            var themeRulesMock = new Mock<IThemeRules>();
            var validator = new TemplateDetailsValidator<TemplateDetails>(templateRulesMock.Object, themeRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.ViewName, command);
        }

        [Test]
        public void Should_have_validation_error_when_view_name_is_too_long()
        {
            var viewName = "";
            for (int i = 0; i < 251; i++) viewName += i;

            var command = new TemplateDetails
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                ViewName = viewName
            };

            var templateRulesMock = new Mock<ITemplateRules>();
            var themeRulesMock = new Mock<IThemeRules>();
            var validator = new TemplateDetailsValidator<TemplateDetails>(templateRulesMock.Object, themeRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.ViewName, command);
        }

        [Test]
        public void Should_have_validation_error_when_view_name_is_not_valid()
        {
            var command = new TemplateDetails
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                ViewName = "a@b"
            };

            var templateRulesMock = new Mock<ITemplateRules>();
            templateRulesMock.Setup(x => x.IsTemplateViewNameValid(command.Description)).Returns(false);

            var themeRulesMock = new Mock<IThemeRules>();

            var validator = new TemplateDetailsValidator<TemplateDetails>(templateRulesMock.Object, themeRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.ViewName, command);
        }

        [Test]
        public void Should_have_validation_error_when_view_name_is_not_unique()
        {
            var command = new TemplateDetails
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                ViewName = "viewName"
            };

            var templateRulesMock = new Mock<ITemplateRules>();
            templateRulesMock.Setup(x => x.IsTemplateViewNameUnique(command.Description, Guid.Empty)).Returns(false);

            var themeRulesMock = new Mock<IThemeRules>();

            var validator = new TemplateDetailsValidator<TemplateDetails>(templateRulesMock.Object, themeRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.ViewName, command);
        }
    }
}
