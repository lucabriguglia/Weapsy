using System;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Pages.Commands;
using Weapsy.Domain.Sites.Rules;
using Weapsy.Domain.Pages.Validators;

namespace Weapsy.Domain.Tests.Pages.Validators
{
    [TestFixture]
    public class HidePageValidatorTests
    {
        [Test]
        public void Should_have_validation_error_when_site_id_is_empty()
        {
            var command = new HidePage
            {
                SiteId = Guid.Empty
            };

            var siteRulesMock = new Mock<ISiteRules>();
            var validator = new HidePageValidator(siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.SiteId, command);
        }

        [Test]
        public void Should_have_validation_error_when_site_does_not_exist()
        {
            var command = new HidePage
            {
                SiteId = Guid.NewGuid()
            };

            var siteRulesMock = new Mock<ISiteRules>();
            siteRulesMock.Setup(x => x.DoesSiteExist(command.SiteId)).Returns(false);

            var validator = new HidePageValidator(siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.SiteId, command);
        }
    }
}
