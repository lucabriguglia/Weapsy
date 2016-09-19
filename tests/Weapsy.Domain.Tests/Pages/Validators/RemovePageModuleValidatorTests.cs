using System;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Sites.Rules;
using Weapsy.Domain.Pages.Commands;
using Weapsy.Domain.Pages.Validators;

namespace Weapsy.Domain.Tests.PageModules.Validators
{
    [TestFixture]
    public class RemovePageModuleValidatorTests
    {
        [Test]
        public void Should_have_validation_error_when_site_id_is_empty()
        {
            var command = new RemovePageModule
            {
                SiteId = Guid.Empty
            };

            var siteRulesMock = new Mock<ISiteRules>();
            var validator = new RemovePageModuleValidator(siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.SiteId, command);
        }

        [Test]
        public void Should_have_validation_error_when_site_does_not_exist()
        {
            var command = new RemovePageModule
            {
                SiteId = Guid.NewGuid()
            };

            var siteRulesMock = new Mock<ISiteRules>();
            siteRulesMock.Setup(x => x.DoesSiteExist(command.SiteId)).Returns(false);

            var validator = new RemovePageModuleValidator(siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.SiteId, command);
        }
    }
}
