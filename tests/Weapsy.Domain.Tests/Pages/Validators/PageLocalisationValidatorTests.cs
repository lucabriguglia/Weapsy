using System;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Pages.Validators;
using Weapsy.Domain.Pages.Rules;
using Weapsy.Domain.Languages.Rules;
using Weapsy.Domain.Pages;

namespace Weapsy.Domain.Tests.Pages.Validators
{
    [TestFixture]
    public class PageLocalisationValidatorTests
    {
        [Test]
        public void Should_have_error_when_language_does_not_exist()
        {
            var pageRulesMock = new Mock<IPageRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            languageRulesMock.Setup(x => x.DoesLanguageExist(It.IsAny<Guid>())).Returns(false);
            var validator = new PageLocalisationValidator(pageRulesMock.Object, languageRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.LanguageId, new PageLocalisation
            {
                LanguageId = Guid.NewGuid(),
                Url = "Url",
                Title = "Title",
                MetaDescription = "Meta Description",
                MetaKeywords = "Meta Keywords"
            });
        }

        [Test]
        public void Should_have_error_when_page_url_is_too_long()
        {
            var pageRulesMock = new Mock<IPageRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var validator = new PageLocalisationValidator(pageRulesMock.Object, languageRulesMock.Object);

            var url = "";
            for (int i = 0; i < 201; i++) url += i;

            validator.ShouldHaveValidationErrorFor(x => x.Url, new PageLocalisation
            {
                LanguageId = Guid.NewGuid(),
                Url = url,
                Title = "Title",
                MetaDescription = "Meta Description",
                MetaKeywords = "Meta Keywords"
            });
        }

        [Test]
        public void Should_have_error_when_page_url_is_not_valid()
        {
            const string url = "My@Url";

            var pageRulesMock = new Mock<IPageRules>();
            pageRulesMock.Setup(x => x.IsPageUrlValid(url)).Returns(false);

            var languageRulesMock = new Mock<ILanguageRules>();
            var validator = new PageLocalisationValidator(pageRulesMock.Object, languageRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Url, new PageLocalisation
            {
                LanguageId = Guid.NewGuid(),
                Url = url,
                Title = "Title",
                MetaDescription = "Meta Description",
                MetaKeywords = "Meta Keywords"
            });
        }

        [Test]
        public void Should_have_error_when_head_title_is_too_long()
        {
            var pageRulesMock = new Mock<IPageRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var validator = new PageLocalisationValidator(pageRulesMock.Object, languageRulesMock.Object);

            var text = "";
            for (int i = 0; i < 251; i++) text += i;

            validator.ShouldHaveValidationErrorFor(x => x.Title, new PageLocalisation
            {
                LanguageId = Guid.NewGuid(),
                Url = "Url",
                Title = text,
                MetaDescription = "Meta Description",
                MetaKeywords = "Meta Keywords"
            });
        }

        [Test]
        public void Should_have_error_when_meta_description_is_too_long()
        {
            var pageRulesMock = new Mock<IPageRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var validator = new PageLocalisationValidator(pageRulesMock.Object, languageRulesMock.Object);

            var text = "";
            for (int i = 0; i < 501; i++) text += i;

            validator.ShouldHaveValidationErrorFor(x => x.MetaDescription, new PageLocalisation
            {
                LanguageId = Guid.NewGuid(),
                Url = "Url",
                Title = "Title",
                MetaDescription = text,
                MetaKeywords = "Meta Keywords"
            });
        }

        [Test]
        public void Should_have_error_when_meta_keywords_is_too_long()
        {
            var pageRulesMock = new Mock<IPageRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var validator = new PageLocalisationValidator(pageRulesMock.Object, languageRulesMock.Object);

            var text = "";
            for (int i = 0; i < 501; i++) text += i;

            validator.ShouldHaveValidationErrorFor(x => x.MetaKeywords, new PageLocalisation
            {
                LanguageId = Guid.NewGuid(),
                Url = "Url",
                Title = "Title",
                MetaDescription = "Meta Description",
                MetaKeywords = text
            });
        }
    }
}
