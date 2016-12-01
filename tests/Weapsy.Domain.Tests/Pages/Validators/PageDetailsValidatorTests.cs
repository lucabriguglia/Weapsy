using System;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Pages.Commands;
using Weapsy.Domain.Pages.Validators;
using Weapsy.Domain.Pages.Rules;
using Weapsy.Domain.Sites.Rules;
using FluentValidation;
using Weapsy.Domain.Languages.Rules;
using System.Collections.Generic;
using Weapsy.Domain.Pages;

namespace Weapsy.Domain.Tests.Pages.Validators
{
    [TestFixture]
    public class PageDetailsValidatorTests
    {
        [Test]
        public void Should_have_error_when_page_name_is_empty()
        {
            var pageRulesMock = new Mock<IPageRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<PageLocalisation>>();
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
            var localisationValidatorMock = new Mock<IValidator<PageLocalisation>>();
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
            var localisationValidatorMock = new Mock<IValidator<PageLocalisation>>();
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
            var localisationValidatorMock = new Mock<IValidator<PageLocalisation>>();
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
            var localisationValidatorMock = new Mock<IValidator<PageLocalisation>>();
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
            var localisationValidatorMock = new Mock<IValidator<PageLocalisation>>();
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
            var localisationValidatorMock = new Mock<IValidator<PageLocalisation>>();
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
            var localisationValidatorMock = new Mock<IValidator<PageLocalisation>>();
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
            var localisationValidatorMock = new Mock<IValidator<PageLocalisation>>();
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
        public void Should_have_error_when_page_slug_already_exists()
        {
            Guid siteId = Guid.NewGuid();
            Guid pageId = Guid.NewGuid();
            const string slug = "something";

            var pageRulesMock = new Mock<IPageRules>();
            pageRulesMock.Setup(x => x.IsSlugUnique(Guid.NewGuid(), slug, Guid.Empty)).Returns(false);

            var siteRulesMock = new Mock<ISiteRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<PageLocalisation>>();
            var validator = new PageDetailsValidator<PageDetails>(pageRulesMock.Object, siteRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Url, new PageDetails
            {
                SiteId = siteId,
                Id = pageId,
                Name = "Name",
                Url = slug,
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
            var localisationValidatorMock = new Mock<IValidator<PageLocalisation>>();
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
            var localisationValidatorMock = new Mock<IValidator<PageLocalisation>>();
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
            var localisationValidatorMock = new Mock<IValidator<PageLocalisation>>();
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
            var localisationValidatorMock = new Mock<IValidator<PageLocalisation>>();
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

        [Test]
        public void Should_have_error_when_localised_page_slug_is_not_unique()
        {
            var pageId = Guid.NewGuid();
            const string slug = "My@Url";

            var pageRulesMock = new Mock<IPageRules>();
            pageRulesMock.Setup(x => x.IsSlugUnique(Guid.NewGuid(), slug, pageId)).Returns(false);
            var siteRulesMock = new Mock<ISiteRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<PageLocalisation>>();
            var validator = new PageDetailsValidator<PageDetails>(pageRulesMock.Object, 
                siteRulesMock.Object, 
                languageRulesMock.Object, 
                localisationValidatorMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.PageLocalisations, new PageDetails());
        }
    }
}
