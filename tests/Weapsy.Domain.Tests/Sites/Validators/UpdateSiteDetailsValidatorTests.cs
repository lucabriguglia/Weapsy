using System;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Model.Sites.Commands;
using Weapsy.Domain.Model.Sites.Validators;
using Weapsy.Domain.Model.Sites.Rules;
using FluentValidation;
using Weapsy.Domain.Model.Languages.Rules;
using System.Collections.Generic;

namespace Weapsy.Domain.Tests.Sites.Validators
{
    [TestFixture]
    public class UpdateSiteDetailsValidatorTests
    {
        [Test]
        public void Should_have_error_when_site_url_is_empty()
        {
            var siteRulesMock = new Mock<ISiteRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<UpdateSiteDetails.SiteLocalisation>>();
            var validator = new UpdateSiteDetailsValidator(siteRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Url, new UpdateSiteDetails
            {
                SiteId = Guid.NewGuid(),
                Url = string.Empty,
                Title = "Title",
                MetaDescription = "Meta Description",
                MetaKeywords = "Meta Keywords"
            });
        }

        [Test]
        public void Should_have_error_when_site_url_is_too_long()
        {
            var siteRulesMock = new Mock<ISiteRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<UpdateSiteDetails.SiteLocalisation>>();
            var validator = new UpdateSiteDetailsValidator(siteRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            var url = "";
            for (int i = 0; i < 51; i++) url += i;

            validator.ShouldHaveValidationErrorFor(x => x.Url, new UpdateSiteDetails
            {
                SiteId = Guid.NewGuid(),
                Url = url,
                Title = "Title",
                MetaDescription = "Meta Description",
                MetaKeywords = "Meta Keywords"
            });
        }

        [Test]
        public void Should_have_error_when_site_url_is_not_valid()
        {
            Guid siteId = Guid.NewGuid();
            const string url = "My@Url";

            var siteRulesMock = new Mock<ISiteRules>();
            siteRulesMock.Setup(x => x.IsSiteUrlValid(url)).Returns(false);

            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<UpdateSiteDetails.SiteLocalisation>>();
            var validator = new UpdateSiteDetailsValidator(siteRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Url, new UpdateSiteDetails
            {
                SiteId = Guid.NewGuid(),
                Url = url,
                Title = "Title",
                MetaDescription = "Meta Description",
                MetaKeywords = "Meta Keywords"
            });
        }

        [Test]
        public void Should_have_error_when_site_url_already_exists()
        {
            Guid siteId = Guid.NewGuid();
            const string url = "my-url";

            var siteRulesMock = new Mock<ISiteRules>();
            siteRulesMock.Setup(x => x.IsSiteUrlUnique(url, siteId)).Returns(false);

            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<UpdateSiteDetails.SiteLocalisation>>();
            var validator = new UpdateSiteDetailsValidator(siteRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Url, new UpdateSiteDetails
            {
                SiteId = siteId,
                Url = url,
                Title = "Title",
                MetaDescription = "Meta Description",
                MetaKeywords = "Meta Keywords"
            });
        }

        [Test]
        public void Should_have_error_when_site_head_title_is_too_long()
        {
            var siteRulesMock = new Mock<ISiteRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<UpdateSiteDetails.SiteLocalisation>>();
            var validator = new UpdateSiteDetailsValidator(siteRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            var title = "";
            for (int i = 0; i < 251; i++) title += i;

            validator.ShouldHaveValidationErrorFor(x => x.Title, new UpdateSiteDetails
            {
                SiteId = Guid.NewGuid(),
                Url = "url",
                Title = title,
                MetaDescription = string.Empty,
                MetaKeywords = string.Empty
            });
        }

        [Test]
        public void Should_have_error_when_site_meta_description_is_too_long()
        {
            var siteRulesMock = new Mock<ISiteRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<UpdateSiteDetails.SiteLocalisation>>();
            var validator = new UpdateSiteDetailsValidator(siteRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            var metaDescription = "";
            for (int i = 0; i < 501; i++) metaDescription += i;

            validator.ShouldHaveValidationErrorFor(x => x.MetaDescription, new UpdateSiteDetails
            {
                SiteId = Guid.NewGuid(),
                Url = "url",
                Title = string.Empty,
                MetaDescription = metaDescription,
                MetaKeywords = string.Empty
            });
        }

        [Test]
        public void Should_have_error_when_site_meta_keywords_is_too_long()
        {
            var siteRulesMock = new Mock<ISiteRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<UpdateSiteDetails.SiteLocalisation>>();
            var validator = new UpdateSiteDetailsValidator(siteRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            var metaKeywords = "";
            for (int i = 0; i < 501; i++) metaKeywords += i;

            validator.ShouldHaveValidationErrorFor(x => x.MetaKeywords, new UpdateSiteDetails
            {
                SiteId = Guid.NewGuid(),
                Url = "url",
                Title = string.Empty,
                MetaDescription = string.Empty,
                MetaKeywords = metaKeywords
            });
        }

        [Test]
        public void Should_have_error_when_localisations_are_missing()
        {
            var siteRulesMock = new Mock<ISiteRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<UpdateSiteDetails.SiteLocalisation>>();
            languageRulesMock.Setup(x => x.AreAllSupportedLanguagesIncluded(It.IsAny<Guid>(), It.IsAny<IEnumerable<Guid>>())).Returns(false);
            var validator = new UpdateSiteDetailsValidator(siteRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.SiteLocalisations, new UpdateSiteDetails
            {
                SiteId = Guid.NewGuid(),
                Title = "Title",
                Url = "Url"
            });
        }
    }
}
