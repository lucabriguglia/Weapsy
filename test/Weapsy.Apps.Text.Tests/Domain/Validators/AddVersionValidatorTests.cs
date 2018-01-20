using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Weapsy.Apps.Text.Domain.Commands;
using Weapsy.Apps.Text.Domain.Rules;
using Weapsy.Apps.Text.Domain.Validators;
using Weapsy.Domain.Languages.Rules;
using Weapsy.Domain.Modules.Rules;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Apps.Text.Tests.Domain.Validators
{
    [TestFixture]
    public class AddVersionValidatorTests
    {
        [Test]
        public void Should_have_error_when_module_id_is_empty()
        {
            var command = new AddVersion
            {
                SiteId = Guid.NewGuid(),
                ModuleId = Guid.Empty,
                Id = Guid.NewGuid(),
                Content = "Content"
            };

            var textModuleRulesMock = new Mock<ITextModuleRules>();
            var moduleRulesMock = new Mock<IModuleRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidator = new Mock<IValidator<AddVersion.VersionLocalisation>>();
            var validator = new AddVersionValidator(textModuleRulesMock.Object, 
                moduleRulesMock.Object, 
                siteRulesMock.Object, 
                languageRulesMock.Object,
                localisationValidator.Object);

            validator.ShouldHaveValidationErrorFor(x => x.ModuleId, command);
        }

        [Test]
        public void Should_have_error_when_module_does_not_exist()
        {
            var command = new AddVersion
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
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidator = new Mock<IValidator<AddVersion.VersionLocalisation>>();
            var validator = new AddVersionValidator(textModuleRulesMock.Object,
                moduleRulesMock.Object,
                siteRulesMock.Object,
                languageRulesMock.Object,
                localisationValidator.Object);

            validator.ShouldHaveValidationErrorFor(x => x.ModuleId, command);
        }

        [Test]
        public void Should_have_error_when_content_is_empty()
        {
            var command = new AddVersion
            {
                SiteId = Guid.NewGuid(),
                ModuleId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Content = ""
            };

            var textModuleRulesMock = new Mock<ITextModuleRules>();
            var moduleRulesMock = new Mock<IModuleRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidator = new Mock<IValidator<AddVersion.VersionLocalisation>>();
            var validator = new AddVersionValidator(textModuleRulesMock.Object,
                moduleRulesMock.Object,
                siteRulesMock.Object,
                languageRulesMock.Object,
                localisationValidator.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Content, command);
        }

        [Test]
        public void Should_have_error_when_description_is_too_long()
        {
            var description = string.Empty;
            for (int i = 0; i < 251; i++) description += i.ToString();

            var command = new AddVersion
            {
                SiteId = Guid.NewGuid(),
                ModuleId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Content = "",
                Description = description
            };

            var textModuleRulesMock = new Mock<ITextModuleRules>();
            var moduleRulesMock = new Mock<IModuleRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidator = new Mock<IValidator<AddVersion.VersionLocalisation>>();
            var validator = new AddVersionValidator(textModuleRulesMock.Object,
                moduleRulesMock.Object,
                siteRulesMock.Object,
                languageRulesMock.Object,
                localisationValidator.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Description, command);
        }

        [Test]
        public void Should_have_error_when_localisations_are_missing()
        {
            var textModuleRulesMock = new Mock<ITextModuleRules>();
            var moduleRulesMock = new Mock<IModuleRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            languageRulesMock.Setup(x => x.AreAllSupportedLanguagesIncluded(It.IsAny<Guid>(), It.IsAny<IEnumerable<Guid>>())).Returns(false);
            var localisationValidator = new Mock<IValidator<AddVersion.VersionLocalisation>>();
            var validator = new AddVersionValidator(textModuleRulesMock.Object,
                moduleRulesMock.Object,
                siteRulesMock.Object,
                languageRulesMock.Object,
                localisationValidator.Object);

            validator.ShouldHaveValidationErrorFor(x => x.VersionLocalisations, new AddVersion
            {
                SiteId = Guid.NewGuid(),
                ModuleId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Content = "Content"
            });
        }
    }
}
