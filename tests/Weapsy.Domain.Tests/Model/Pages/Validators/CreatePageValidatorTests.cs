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
using Weapsy.Domain.Model.Pages;

namespace Weapsy.Domain.Tests.Pages.Validators
{
    [TestFixture]
    public class CreatePageValidatorTests
    {
        [Test]
        public void Should_have_error_when_page_id_is_empty()
        {
            var pageRulesMock = new Mock<IPageRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<PageLocalisation>>();
            var validator = new CreatePageValidator(pageRulesMock.Object, siteRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Id, new CreatePage
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.Empty,
                Name = "Name",
                Url = "url"
            });
        }

        [Test]
        public void Should_have_error_when_page_id_already_exists()
        {
            Guid id = Guid.NewGuid();

            var pageRulesMock = new Mock<IPageRules>();
            pageRulesMock.Setup(x => x.IsPageIdUnique(id)).Returns(false);

            var siteRulesMock = new Mock<ISiteRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<PageLocalisation>>();
            var validator = new CreatePageValidator(pageRulesMock.Object, siteRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Id, new CreatePage
            {
                SiteId = Guid.NewGuid(),
                Id = id,
                Name = "Name",
                Url = "url"
            });
        }      
    }
}
