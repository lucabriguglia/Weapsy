using System;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Model.Languages.Commands;
using Weapsy.Domain.Model.Sites.Rules;
using Weapsy.Domain.Model.Languages.Validators;

namespace Weapsy.Domain.Tests.Languages.Validators
{
    [TestFixture]
    public class DeleteLanguageValidatorTests
    {
        [Test]
        public void Should_have_validation_error_when_site_id_is_empty()
        {
            var command = new DeleteLanguage
            {
                SiteId = Guid.Empty,
                Id = Guid.NewGuid()
            };

            var siteRulesMock = new Mock<ISiteRules>();
            var validator = new DeleteLanguageValidator(siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.SiteId, command);
        }

        [Test]
        public void Should_have_validation_error_when_site_does_not_exist()
        {
            var command = new DeleteLanguage
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid()
            };

            var siteRulesMock = new Mock<ISiteRules>();
            siteRulesMock.Setup(x => x.DoesSiteExist(command.SiteId)).Returns(false);

            var validator = new DeleteLanguageValidator(siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.SiteId, command);
        }
    }
}
