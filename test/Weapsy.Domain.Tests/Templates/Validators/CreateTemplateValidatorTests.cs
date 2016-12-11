using System;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Templates.Commands;
using Weapsy.Domain.Templates.Validators;
using Weapsy.Domain.Templates.Rules;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Tests.Templates.Validators
{
    [TestFixture]
    public class CreateTemplateValidatorTests
    {
        [Test]
        public void Should_have_validation_error_when_template_id_already_exists()
        {
            var command = new CreateTemplate
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                ViewName = "ViewName"
            };

            var templateRulesMock = new Mock<ITemplateRules>();
            templateRulesMock.Setup(x => x.IsTemplateIdUnique(command.Id)).Returns(false);

            var siteRulesMock = new Mock<ISiteRules>();
            var validator = new CreateTemplateValidator(templateRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Id, command);
        }

        [Test]
        public void Should_have_validation_error_when_template_name_is_empty()
        {
            var command = new CreateTemplate
            {
                Id = Guid.NewGuid(),
                Name = string.Empty,
                Description = "Description",
                ViewName = "ViewName"
            };

            var templateRulesMock = new Mock<ITemplateRules>();
            var validator = new CreateTemplateValidator(templateRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Test]
        public void Should_have_validation_error_when_template_name_is_too_long()
        {
            var name = "";
            for (int i = 0; i < 101; i++) name += i;

            var command = new CreateTemplate
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = "Description",
                ViewName = "ViewName"
            };

            var templateRulesMock = new Mock<ITemplateRules>();
            var validator = new CreateTemplateValidator(templateRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Test]
        public void Should_have_validation_error_when_template_name_is_not_unique()
        {
            var command = new CreateTemplate
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                ViewName = "ViewName"
            };

            var templateRulesMock = new Mock<ITemplateRules>();
            templateRulesMock.Setup(x => x.IsTemplateNameUnique(command.Name, Guid.Empty)).Returns(false);

            var validator = new CreateTemplateValidator(templateRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Test]
        public void Should_have_validation_error_when_description_is_too_long()
        {
            var description = "";
            for (int i = 0; i < 251; i++) description += i;

            var command = new CreateTemplate
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = description,
                ViewName = "ViewName"
            };

            var templateRulesMock = new Mock<ITemplateRules>();
            var validator = new CreateTemplateValidator(templateRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Description, command);
        }

        [Test]
        public void Should_have_validation_error_when_view_name_is_empty()
        {
            var command = new CreateTemplate
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                ViewName = string.Empty
            };

            var templateRulesMock = new Mock<ITemplateRules>();
            var validator = new CreateTemplateValidator(templateRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.ViewName, command);
        }

        [Test]
        public void Should_have_validation_error_when_view_name_is_too_long()
        {
            var viewName = "";
            for (int i = 0; i < 251; i++) viewName += i;

            var command = new CreateTemplate
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                ViewName = viewName
            };

            var templateRulesMock = new Mock<ITemplateRules>();
            var validator = new CreateTemplateValidator(templateRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.ViewName, command);
        }

        [Test]
        public void Should_have_validation_error_when_view_name_is_not_valid()
        {
            var command = new CreateTemplate
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                ViewName = "a@b"
            };

            var templateRulesMock = new Mock<ITemplateRules>();
            templateRulesMock.Setup(x => x.IsTemplateViewNameValid(command.ViewName)).Returns(false);

            var validator = new CreateTemplateValidator(templateRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.ViewName, command);
        }

        [Test]
        public void Should_have_validation_error_when_view_name_is_not_unique()
        {
            var command = new CreateTemplate
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                ViewName = "ViewName"
            };

            var templateRulesMock = new Mock<ITemplateRules>();
            templateRulesMock.Setup(x => x.IsTemplateViewNameUnique(command.ViewName, Guid.Empty)).Returns(false);

            var validator = new CreateTemplateValidator(templateRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.ViewName, command);
        }
    }
}
