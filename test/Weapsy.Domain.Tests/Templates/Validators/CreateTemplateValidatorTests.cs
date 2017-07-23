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

            var themeRulesMock = new Mock<IThemeRules>();

            var validator = new CreateTemplateValidator(templateRulesMock.Object, themeRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Id, command);
        }
    }
}
