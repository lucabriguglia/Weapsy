using System;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Weapsy.Apps.Text.Domain.Commands;
using Weapsy.Apps.Text.Domain.Rules;
using Weapsy.Apps.Text.Domain.Validators;
using Weapsy.Domain.Modules.Rules;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Apps.Text.Tests.Domain.Validators
{
    [TestFixture]
    public class CreateTextModuleValidatorTests
    {
        [Test]
        public void Should_have_error_when_text_module_id_is_empty()
        {
            var command = new CreateTextModule
            {
                SiteId = Guid.NewGuid(),
                ModuleId = Guid.NewGuid(),
                Id = Guid.Empty,
                Content = "Content"
            };

            var textModuleRulesMock = new Mock<ITextModuleRules>();
            var moduleRulesMock = new Mock<IModuleRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var validator = new CreateTextModuleValidator(textModuleRulesMock.Object, moduleRulesMock.Object, siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Id, command);
        }

        [Test]
        public void Should_have_error_when_text_module_id_already_exists()
        {
            var command = new CreateTextModule
            {
                SiteId = Guid.NewGuid(),
                ModuleId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Content = "Content"
            };

            var textModuleRulesMock = new Mock<ITextModuleRules>();
            textModuleRulesMock.Setup(x => x.IsTextModuleIdUnique(command.Id)).Returns(false);
            var moduleRulesMock = new Mock<IModuleRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var validator = new CreateTextModuleValidator(textModuleRulesMock.Object, moduleRulesMock.Object, siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Id, command);
        }

        [Test]
        public void Should_have_error_when_module_id_is_empty()
        {
            var command = new CreateTextModule
            {
                SiteId = Guid.NewGuid(),
                ModuleId = Guid.Empty,
                Id = Guid.NewGuid(),
                Content = "Content"
            };

            var textModuleRulesMock = new Mock<ITextModuleRules>();
            var moduleRulesMock = new Mock<IModuleRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var validator = new CreateTextModuleValidator(textModuleRulesMock.Object, moduleRulesMock.Object, siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Id, command);
        }

        [Test]
        public void Should_have_error_when_module_does_not_exist()
        {
            var command = new CreateTextModule
            {
                SiteId = Guid.NewGuid(),
                ModuleId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Content = "Content"
            };

            var textModuleRulesMock = new Mock<ITextModuleRules>();            
            var moduleRulesMock = new Mock<IModuleRules>();
            moduleRulesMock.Setup(x => x.DoesModuleExist(command.SiteId, command.ModuleId)).Returns(false);
            var siteRulesMock = new Mock<ISiteRules>();
            var validator = new CreateTextModuleValidator(textModuleRulesMock.Object, moduleRulesMock.Object, siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.ModuleId, command);
        }

        [Test]
        public void Should_have_error_when_text_module_content_is_empty()
        {
            var command = new CreateTextModule
            {
                SiteId = Guid.NewGuid(),
                ModuleId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Content = ""
            };

            var textModuleRulesMock = new Mock<ITextModuleRules>();
            var moduleRulesMock = new Mock<IModuleRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var validator = new CreateTextModuleValidator(textModuleRulesMock.Object, moduleRulesMock.Object, siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Content, command);
        }
    }
}
