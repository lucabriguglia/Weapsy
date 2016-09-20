using System;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Languages.Rules;
using Weapsy.Apps.Text.Domain.Commands;
using Weapsy.Apps.Text.Domain.Validators;

namespace Weapsy.Apps.Text.Domain.Tests.Validators
{
    [TestFixture]
    public class VersionLocalisationValidatorTests
    {
        [Test]
        public void Should_have_error_when_language_id_is_empty()
        {
            var languageRulesMock = new Mock<ILanguageRules>();
            var validator = new VersionLocalisationValidator(languageRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.LanguageId, new AddVersion.VersionLocalisation
            {
                LanguageId = Guid.Empty,
                Content = "Content"
            });
        }

        [Test]
        public void Should_have_error_when_language_does_not_exist()
        {
            var languageRulesMock = new Mock<ILanguageRules>();
            languageRulesMock.Setup(x => x.DoesLanguageExist(It.IsAny<Guid>())).Returns(false);
            var validator = new VersionLocalisationValidator(languageRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.LanguageId, new AddVersion.VersionLocalisation
            {
                LanguageId = Guid.NewGuid(),
                Content = "Content"
            });
        }

        [Test]
        public void Should_have_error_when_content_is_too_long()
        {
            var languageRulesMock = new Mock<ILanguageRules>();
            var validator = new VersionLocalisationValidator(languageRulesMock.Object);

            var content = "";
            for (int i = 0; i < 251; i++) content += i;

            validator.ShouldHaveValidationErrorFor(x => x.Content, new AddVersion.VersionLocalisation
            {
                LanguageId = Guid.NewGuid(),
                Content = content
            });
        }
    }
}
