using System;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Languages.Commands;
using Weapsy.Domain.Languages.Validators;
using Weapsy.Domain.Languages.Rules;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Tests.Languages.Validators
{
    [TestFixture]
    public class LanguageDetailsValidatorTests
    {
        [Test]
        public void Should_have_validation_error_when_language_name_is_empty()
        {
            var command = new LanguageDetails
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My Language",
                CultureName = "aa-bb",
                Url = "url"
            };

            var languageRulesMock = new Mock<ILanguageRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var validator = new LanguageDetailsValidator<LanguageDetails>(languageRulesMock.Object, siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Test]
        public void Should_have_validation_error_when_language_name_is_too_short()
        {
            var command = new LanguageDetails
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "A",
                CultureName = "aa-bb",
                Url = "url"
            };

            var languageRulesMock = new Mock<ILanguageRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var validator = new LanguageDetailsValidator<LanguageDetails>(languageRulesMock.Object, siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Test]
        public void Should_have_validation_error_when_language_name_is_too_long()
        {
            var name = "";
            for (int i = 0; i < 101; i++) name += i;

            var command = new LanguageDetails
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = name,
                CultureName = "aa-bb",
                Url = "url"
            };

            var languageRulesMock = new Mock<ILanguageRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var validator = new LanguageDetailsValidator<LanguageDetails>(languageRulesMock.Object, siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Test]
        public void Should_have_validation_error_when_language_name_is_not_valid()
        {
            var command = new LanguageDetails
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My@Language",
                CultureName = "aa-bb",
                Url = "url"
            };

            var languageRulesMock = new Mock<ILanguageRules>();
            languageRulesMock.Setup(x => x.IsLanguageNameValid(command.Name)).Returns(false);

            var siteRulesMock = new Mock<ISiteRules>();
            var validator = new LanguageDetailsValidator<LanguageDetails>(languageRulesMock.Object, siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Test]
        public void Should_have_validation_error_when_language_name_is_not_unique()
        {
            var command = new LanguageDetails
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My Language",
                CultureName = "aa-bb",
                Url = "url"
            };

            var languageRulesMock = new Mock<ILanguageRules>();
            languageRulesMock.Setup(x => x.IsLanguageNameUnique(command.SiteId, command.Name, Guid.Empty)).Returns(false);

            var siteRulesMock = new Mock<ISiteRules>();
            var validator = new LanguageDetailsValidator<LanguageDetails>(languageRulesMock.Object, siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Test]
        public void Should_have_validation_error_when_culture_name_is_empty()
        {
            var command = new LanguageDetails
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My Language",
                CultureName = string.Empty,
                Url = "url"
            };

            var languageRulesMock = new Mock<ILanguageRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var validator = new LanguageDetailsValidator<LanguageDetails>(languageRulesMock.Object, siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.CultureName, command);
        }

        [Test]
        public void Should_have_validation_error_when_culture_name_is_too_short()
        {
            var command = new LanguageDetails
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My Language",
                CultureName = "a",
                Url = "url"
            };

            var languageRulesMock = new Mock<ILanguageRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var validator = new LanguageDetailsValidator<LanguageDetails>(languageRulesMock.Object, siteRulesMock.Object); ;

            validator.ShouldHaveValidationErrorFor(x => x.CultureName, command);
        }

        [Test]
        public void Should_have_validation_error_when_culture_name_is_too_long()
        {
            var cultureName = "";
            for (int i = 0; i < 101; i++) cultureName += i;

            var command = new LanguageDetails
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My Language",
                CultureName = cultureName,
                Url = "url"
            };

            var languageRulesMock = new Mock<ILanguageRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var validator = new LanguageDetailsValidator<LanguageDetails>(languageRulesMock.Object, siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.CultureName, command);
        }

        [Test]
        public void Should_have_validation_error_when_culture_name_is_not_valid()
        {
            var command = new LanguageDetails
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My Language",
                CultureName = "a@b",
                Url = "url"
            };

            var languageRulesMock = new Mock<ILanguageRules>();
            languageRulesMock.Setup(x => x.IsCultureNameValid(command.CultureName)).Returns(false);

            var siteRulesMock = new Mock<ISiteRules>();
            var validator = new LanguageDetailsValidator<LanguageDetails>(languageRulesMock.Object, siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.CultureName, command);
        }

        [Test]
        public void Should_have_validation_error_when_culture_name_is_not_unique()
        {
            var command = new LanguageDetails
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My Language",
                CultureName = "aa-bb",
                Url = "url"
            };

            var languageRulesMock = new Mock<ILanguageRules>();
            languageRulesMock.Setup(x => x.IsCultureNameUnique(command.SiteId, command.CultureName, Guid.Empty)).Returns(false);

            var siteRulesMock = new Mock<ISiteRules>();
            var validator = new LanguageDetailsValidator<LanguageDetails>(languageRulesMock.Object, siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.CultureName, command);
        }

        [Test]
        public void Should_have_validation_error_when_url_is_empty()
        {
            var command = new LanguageDetails
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "Name",
                CultureName = "aa-bb",
                Url = string.Empty
            };

            var languageRulesMock = new Mock<ILanguageRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var validator = new LanguageDetailsValidator<LanguageDetails>(languageRulesMock.Object, siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Url, command);
        }

        [Test]
        public void Should_have_validation_error_when_url_is_too_short()
        {
            var command = new LanguageDetails
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My Language",
                CultureName = "aa-bb",
                Url = "a"
            };

            var languageRulesMock = new Mock<ILanguageRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var validator = new LanguageDetailsValidator<LanguageDetails>(languageRulesMock.Object, siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Url, command);
        }

        [Test]
        public void Should_have_validation_error_when_url_is_too_long()
        {
            var url = "";
            for (int i = 0; i < 101; i++) url += i;

            var command = new LanguageDetails
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My Language",
                CultureName = "aa-bb",
                Url = url
            };

            var languageRulesMock = new Mock<ILanguageRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var validator = new LanguageDetailsValidator<LanguageDetails>(languageRulesMock.Object, siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Url, command);
        }

        [Test]
        public void Should_have_validation_error_when_url_is_not_valid()
        {
            var command = new LanguageDetails
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My Language",
                CultureName = "aa-bb",
                Url = "a@b"
            };

            var languageRulesMock = new Mock<ILanguageRules>();
            languageRulesMock.Setup(x => x.IsLanguageUrlValid(command.CultureName)).Returns(false);

            var siteRulesMock = new Mock<ISiteRules>();
            var validator = new LanguageDetailsValidator<LanguageDetails>(languageRulesMock.Object, siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Url, command);
        }

        [Test]
        public void Should_have_validation_error_when_url_is_not_unique()
        {
            var command = new LanguageDetails
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My Language",
                CultureName = "aa-bb",
                Url = "url"
            };

            var languageRulesMock = new Mock<ILanguageRules>();
            languageRulesMock.Setup(x => x.IsLanguageUrlUnique(command.SiteId, command.CultureName, Guid.Empty)).Returns(false);

            var siteRulesMock = new Mock<ISiteRules>();
            var validator = new LanguageDetailsValidator<LanguageDetails>(languageRulesMock.Object, siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Url, command);
        }
    }
}
