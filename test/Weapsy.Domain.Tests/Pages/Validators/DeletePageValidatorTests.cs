using System;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Pages.Commands;
using Weapsy.Domain.Pages.Validators;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Tests.Pages.Validators
{
    [TestFixture]
    public class DeletePageValidatorTests
    {
        [Test]
        public void Should_have_error_when_page_is_set_as_home_page()
        {
            var siteId = Guid.NewGuid();
            var pageId = Guid.NewGuid();

            var siteRulesMock = new Mock<ISiteRules>();
            siteRulesMock.Setup(x => x.IsPageSetAsHomePage(siteId, pageId)).Returns(true);

            var validator = new DeletePageValidator(siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Id, new DeletePage
            {
                SiteId = siteId,
                Id = pageId
            });
        }
    }
}
