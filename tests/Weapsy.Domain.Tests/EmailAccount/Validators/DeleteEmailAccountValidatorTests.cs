using System;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.EmailAccounts.Commands;
using Weapsy.Domain.EmailAccounts.Validators;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Tests.EmailAccount.Validators
{
    [TestFixture]
    public class DeleteEmailAccountValidatorTests
    {
        [Test]
        public void Should_have_validation_error_when_site_id_is_empty()
        {
            var command = new DeleteEmailAccount
            {
                SiteId = Guid.Empty,
                Id = Guid.NewGuid()
            };

            var siteRulesMock = new Mock<ISiteRules>();
            var validator = new DeleteEmailAccountValidator(siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.SiteId, command);
        }

        [Test]
        public void Should_have_validation_error_when_site_does_not_exist()
        {
            var command = new DeleteEmailAccount
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid()
            };

            var siteRulesMock = new Mock<ISiteRules>();
            siteRulesMock.Setup(x => x.DoesSiteExist(command.SiteId)).Returns(false);

            var validator = new DeleteEmailAccountValidator(siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.SiteId, command);
        }
    }
}
