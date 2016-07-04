using System;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Model.Pages.Commands;
using Weapsy.Domain.Model.Pages.Validators;
using Weapsy.Domain.Model.Pages.Rules;
using Weapsy.Domain.Model.Sites.Rules;
using FluentValidation;
using Weapsy.Domain.Model.Languages.Rules;
using System.Collections.Generic;

namespace Weapsy.Domain.Tests.Pages.Validators
{
    [TestFixture]
    public class PageDetailsValidatorTests
    {
        [Test]
        public void Should_have_validation_error_when_site_id_is_empty()
        {
            var command = new PageDetails
            {
                SiteId = Guid.Empty
            };

            var pageRulesMock = new Mock<IPageRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<PageDetails.PageLocalisation>>();
            var validator = new PageDetailsValidator<PageDetails>(pageRulesMock.Object, siteRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.SiteId, command);
        }

        [Test]
        public void Should_have_validation_error_when_site_does_not_exist()
        {
            var command = new PageDetails
            {
                SiteId = Guid.NewGuid()
            };

            var pageRulesMock = new Mock<IPageRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            siteRulesMock.Setup(x => x.DoesSiteExist(command.SiteId)).Returns(false);
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<PageDetails.PageLocalisation>>();
            var validator = new PageDetailsValidator<PageDetails>(pageRulesMock.Object, siteRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.SiteId, command);
        }

        [Test]
        public void Should_have_error_when_page_name_is_empty()
        {
            var pageRulesMock = new Mock<IPageRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<PageDetails.PageLocalisation>>();
            var validator = new PageDetailsValidator<PageDetails>(pageRulesMock.Object, siteRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, new PageDetails
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = string.Empty,
                Url = "url",
                Title = "Title",
                MetaDescription = "Meta Description",
                MetaKeywords = "Meta Keywords"
            });
        }

        [Test]
        public void Should_have_error_when_page_name_is_too_short()
        {
            var pageRulesMock = new Mock<IPageRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<PageDetails.PageLocalisation>>();
            var validator = new PageDetailsValidator<PageDetails>(pageRulesMock.Object, siteRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, new PageDetails
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My",
                Url = "url",
                Title = "Title",
                MetaDescription = "Meta Description",
                MetaKeywords = "Meta Keywords"
            });
        }

        [Test]
        public void Should_have_error_when_page_name_is_too_long()
        {
            var pageRulesMock = new Mock<IPageRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<PageDetails.PageLocalisation>>();
            var validator = new PageDetailsValidator<PageDetails>(pageRulesMock.Object, siteRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            var name = "";
            for (int i = 0; i < 101; i++) name += i;

            validator.ShouldHaveValidationErrorFor(x => x.Name, new PageDetails
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = name,
                Url = "url",
                Title = "Title",
                MetaDescription = "Meta Description",
                MetaKeywords = "Meta Keywords"
            });
        }

        [Test]
        public void Should_have_error_when_page_name_is_not_valid()
        {
            Guid siteId = Guid.NewGuid();
            const string name = "My@Page";

            var pageRulesMock = new Mock<IPageRules>();
            pageRulesMock.Setup(x => x.IsPageNameValid(name)).Returns(false);

            var siteRulesMock = new Mock<ISiteRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<PageDetails.PageLocalisation>>();
            var validator = new PageDetailsValidator<PageDetails>(pageRulesMock.Object, siteRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, new PageDetails
            {
                SiteId = siteId,
                Id = Guid.NewGuid(),
                Name = name,
                Url = "url",
                Title = "Title",
                MetaDescription = "Meta Description",
                MetaKeywords = "Meta Keywords"
            });
        }

        [Test]
        public void Should_have_error_when_page_name_already_exists()
        {
            Guid siteId = Guid.NewGuid();
            Guid pageId = Guid.NewGuid();
            const string name = "My Page";

            var pageRulesMock = new Mock<IPageRules>();
            pageRulesMock.Setup(x => x.IsPageNameUnique(siteId, name, pageId)).Returns(false);

            var siteRulesMock = new Mock<ISiteRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<PageDetails.PageLocalisation>>();
            var validator = new PageDetailsValidator<PageDetails>(pageRulesMock.Object, siteRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, new PageDetails
            {
                SiteId = siteId,
                Id = pageId,
                Name = name,
                Url = "url"
            });
        }

        [Test]
        public void Should_have_error_when_page_url_is_empty()
        {
            var pageRulesMock = new Mock<IPageRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<PageDetails.PageLocalisation>>();
            var validator = new PageDetailsValidator<PageDetails>(pageRulesMock.Object, siteRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Url, new PageDetails
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "Name",
                Url = string.Empty,
                Title = "Title",
                MetaDescription = "Meta Description",
                MetaKeywords = "Meta Keywords"
            });
        }

        [Test]
        public void Should_have_error_when_page_url_is_too_long()
        {
            var pageRulesMock = new Mock<IPageRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<PageDetails.PageLocalisation>>();
            var validator = new PageDetailsValidator<PageDetails>(pageRulesMock.Object, siteRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            var url = "";
            for (int i = 0; i < 201; i++) url += i;

            validator.ShouldHaveValidationErrorFor(x => x.Url, new PageDetails
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "Name",
                Url = url,
                Title = "Title",
                MetaDescription = "Meta Description",
                MetaKeywords = "Meta Keywords"
            });
        }

        [Test]
        public void Should_have_error_when_page_url_is_not_valid()
        {
            Guid siteId = Guid.NewGuid();
            const string url = "My@Url";

            var pageRulesMock = new Mock<IPageRules>();
            pageRulesMock.Setup(x => x.IsPageUrlValid(url)).Returns(false);

            var siteRulesMock = new Mock<ISiteRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<PageDetails.PageLocalisation>>();
            var validator = new PageDetailsValidator<PageDetails>(pageRulesMock.Object, siteRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Url, new PageDetails
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "Name",
                Url = url,
                Title = "Title",
                MetaDescription = "Meta Description",
                MetaKeywords = "Meta Keywords"
            });
        }

        [Test]
        public void Should_have_error_when_page_url_is_reserved()
        {
            const string url = "admin";

            var pageRulesMock = new Mock<IPageRules>();
            pageRulesMock.Setup(x => x.IsPageUrlReserved(url)).Returns(true);

            var siteRulesMock = new Mock<ISiteRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<PageDetails.PageLocalisation>>();
            var validator = new PageDetailsValidator<PageDetails>(pageRulesMock.Object, siteRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Url, new PageDetails
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "Name",
                Url = url
            });
        }

        [Test]
        public void Should_have_error_when_page_url_already_exists()
        {
            Guid siteId = Guid.NewGuid();
            Guid pageId = Guid.NewGuid();
            const string url = "my-url";

            var pageRulesMock = new Mock<IPageRules>();
            pageRulesMock.Setup(x => x.IsPageUrlUnique(siteId, url, pageId)).Returns(false);

            var siteRulesMock = new Mock<ISiteRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<PageDetails.PageLocalisation>>();
            var validator = new PageDetailsValidator<PageDetails>(pageRulesMock.Object, siteRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Url, new PageDetails
            {
                SiteId = siteId,
                Id = pageId,
                Name = "Name",
                Url = url,
                Title = "Title",
                MetaDescription = "Meta Description",
                MetaKeywords = "Meta Keywords"
            });
        }

        [Test]
        public void Should_have_error_when_page_head_title_is_too_long()
        {
            var pageRulesMock = new Mock<IPageRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<PageDetails.PageLocalisation>>();
            var validator = new PageDetailsValidator<PageDetails>(pageRulesMock.Object, siteRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            var title = "";
            for (int i = 0; i < 251; i++) title += i;

            validator.ShouldHaveValidationErrorFor(x => x.Title, new PageDetails
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "Name",
                Url = "url",
                Title = title,
                MetaDescription = string.Empty,
                MetaKeywords = string.Empty
            });
        }

        [Test]
        public void Should_have_error_when_page_meta_description_is_too_long()
        {
            var pageRulesMock = new Mock<IPageRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<PageDetails.PageLocalisation>>();
            var validator = new PageDetailsValidator<PageDetails>(pageRulesMock.Object, siteRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            var metaDescription = "";
            for (int i = 0; i < 501; i++) metaDescription += i;

            validator.ShouldHaveValidationErrorFor(x => x.MetaDescription, new PageDetails
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "Name",
                Url = "url",
                Title = string.Empty,
                MetaDescription = metaDescription,
                MetaKeywords = string.Empty
            });
        }

        [Test]
        public void Should_have_error_when_page_meta_keywords_is_too_long()
        {
            var pageRulesMock = new Mock<IPageRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<PageDetails.PageLocalisation>>();
            var validator = new PageDetailsValidator<PageDetails>(pageRulesMock.Object, siteRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            var metaKeywords = "";
            for (int i = 0; i < 251; i++) metaKeywords += i;

            validator.ShouldHaveValidationErrorFor(x => x.MetaKeywords, new PageDetails
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "Name",
                Url = "url",
                Title = string.Empty,
                MetaDescription = string.Empty,
                MetaKeywords = metaKeywords
            });
        }

        [Test]
        public void Should_have_error_when_localisations_are_missing()
        {
            var pageRulesMock = new Mock<IPageRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<PageDetails.PageLocalisation>>();
            languageRulesMock.Setup(x => x.AreAllSupportedLanguagesIncluded(It.IsAny<Guid>(), It.IsAny<IEnumerable<Guid>>())).Returns(false);
            var validator = new PageDetailsValidator<PageDetails>(pageRulesMock.Object, siteRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.PageLocalisations, new PageDetails
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "Name",
                Url = "Url"
            });
        }
    }
}
