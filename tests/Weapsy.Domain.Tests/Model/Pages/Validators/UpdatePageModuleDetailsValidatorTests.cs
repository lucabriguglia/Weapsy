using System;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Model.Pages.Commands;
using Weapsy.Domain.Model.Pages.Validators;
using FluentValidation;
using Weapsy.Domain.Model.Sites.Rules;
using Weapsy.Domain.Model.Pages;

namespace Weapsy.Domain.Tests.Pages.Validators
{
    [TestFixture]
    public class UpdatePageModuleDetailsValidatorTests
    {
        [Test]
        public void Should_have_validation_error_when_site_id_is_empty()
        {
            var command = new UpdatePageModuleDetails
            {
                SiteId = Guid.Empty
            };

            var siteRulesMock = new Mock<ISiteRules>();
            var localisationValidator = new Mock<IValidator<PageModuleLocalisation>>();
            var validator = new UpdatePageModuleDetailsValidator(siteRulesMock.Object, localisationValidator.Object);

            validator.ShouldHaveValidationErrorFor(x => x.SiteId, command);
        }

        [Test]
        public void Should_have_validation_error_when_site_does_not_exist()
        {
            var command = new UpdatePageModuleDetails
            {
                SiteId = Guid.NewGuid()
            };

            var siteRulesMock = new Mock<ISiteRules>();
            siteRulesMock.Setup(x => x.DoesSiteExist(command.SiteId)).Returns(false);
            var localisationValidator = new Mock<IValidator<PageModuleLocalisation>>();
            var validator = new UpdatePageModuleDetailsValidator(siteRulesMock.Object, localisationValidator.Object);

            validator.ShouldHaveValidationErrorFor(x => x.SiteId, command);
        }

        [Test]
        public void Should_have_error_when_page_title_is_too_long()
        {
            var siteRulesMock = new Mock<ISiteRules>();
            var localisationValidator = new Mock<IValidator<PageModuleLocalisation>>();
            var validator = new UpdatePageModuleDetailsValidator(siteRulesMock.Object, localisationValidator.Object);

            var title = "";
            for (int i = 0; i < 251; i++) title += i;

            validator.ShouldHaveValidationErrorFor(x => x.Title, new UpdatePageModuleDetails
            {
                SiteId = Guid.NewGuid(),
                PageId = Guid.NewGuid(),
                ModuleId = Guid.NewGuid(),
                Title = title
            });
        }
    }
}
