using AutoFixture;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Models.Sites.Commands;
using Weapsy.Domain.Services.Sites.Rules;
using Weapsy.Domain.Services.Sites.Validators;

namespace Weapsy.Domain.Services.Tests.Sites.Validators
{
    [TestFixture]
    public class CreateSiteValidatorTests
    {
        [Test]
        public void HasValidationErrorWhenSiteNameIsEmpty()
        {
            var command = new Fixture()
                .Build<CreateSite>()
                .With(x => x.Name, string.Empty)
                .Create();
            var siteRulesMock = new Mock<ISiteRules>();
            siteRulesMock.Setup(x => x.IsNameUniqueAsync(command.Name, command.SiteId)).ReturnsAsync(true);
            var validator = new CreateSiteValidator(siteRulesMock.Object);
            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Test]
        public void HasValidationErrorWhenSiteNameIsTooLong()
        {
            var command = new Fixture()
                .Build<CreateSite>()
                .With(x => x.Name, new string('*', 101))
                .Create();
            var siteRulesMock = new Mock<ISiteRules>();
            siteRulesMock.Setup(x => x.IsNameUniqueAsync(command.Name, command.SiteId)).ReturnsAsync(true);
            var validator = new CreateSiteValidator(siteRulesMock.Object);
            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Test]
        public void HasValidationErrorWhenSiteNameIsNotUnique()
        {
            var command = new Fixture().Create<CreateSite>();
            var siteRulesMock = new Mock<ISiteRules>();
            siteRulesMock.Setup(x => x.IsNameUniqueAsync(command.Name, command.SiteId)).ReturnsAsync(false);
            var validator = new CreateSiteValidator(siteRulesMock.Object);
            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }
    }
}
