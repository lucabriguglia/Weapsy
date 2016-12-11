using System;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Pages.Commands;
using Weapsy.Domain.Pages.Validators;
using FluentValidation;
using Weapsy.Domain.Sites.Rules;
using Weapsy.Domain.Pages;

namespace Weapsy.Domain.Tests.Pages.Validators
{
    [TestFixture]
    public class UpdatePageModuleDetailsValidatorTests
    {
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
