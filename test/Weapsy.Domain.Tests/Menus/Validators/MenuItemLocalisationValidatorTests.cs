using System;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Menus.Validators;
using Weapsy.Domain.Languages.Rules;
using Weapsy.Domain.Menus;

namespace Weapsy.Domain.Tests.Menus.Validators
{
    [TestFixture]
    public class MenuItemLocalisationValidatorTests
    {
        [Test]
        public void Should_have_validation_error_when_language_id_is_empty()
        {
            var command = new MenuItemLocalisation
            {
                LanguageId = Guid.Empty,
                Text = "Text",
                Title = "Title"
            };

            var languageRulesMock = new Mock<ILanguageRules>();
            var validator = new MenuItemLocalisationValidator(languageRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.LanguageId, command);
        }

        [Test]
        public void Should_have_validation_error_when_language_does_not_exist()
        {
            var localisation = new MenuItemLocalisation
            {
                LanguageId = Guid.NewGuid(),
                Text = "Text",
                Title = "Title"
            };

            var languageRulesMock = new Mock<ILanguageRules>();
            languageRulesMock.Setup(x => x.DoesLanguageExist(localisation.LanguageId)).Returns(false);

            var validator = new MenuItemLocalisationValidator(languageRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.LanguageId, localisation);
        }

        [Test]
        public void Should_have_validation_error_when_text_is_too_long()
        {
            var text = string.Empty;
            for (int i = 0; i < 101; i++) text += i;

            var command = new MenuItemLocalisation
            {
                LanguageId = Guid.NewGuid(),
                Text = text,
                Title = "Title"
            };

            var languageRulesMock = new Mock<ILanguageRules>();
            var validator = new MenuItemLocalisationValidator(languageRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Text, command);
        }

        [Test]
        public void Should_have_validation_error_when_title_is_too_long()
        {
            var title = string.Empty;
            for (int i = 0; i < 101; i++) title += i;

            var command = new MenuItemLocalisation
            {
                LanguageId = Guid.NewGuid(),
                Text = "Text",
                Title = title
            };

            var languageRulesMock = new Mock<ILanguageRules>();
            var validator = new MenuItemLocalisationValidator(languageRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Title, command);
        }
    }
}
