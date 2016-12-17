using System;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Sites.Commands;
using Weapsy.Domain.Sites.Validators;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Tests.Sites.Validators
{
    [TestFixture]
    public class CreateSiteValidatorTests
    {
        [Test]
        public void Should_have_error_when_site_id_already_exists()
        {
            var command = new CreateSite
            {
                Id = Guid.NewGuid(),
                Name = "My Site"
            };

            var siteRules = new Mock<ISiteRules>();
            siteRules.Setup(x => x.IsSiteIdUnique(command.Id)).Returns(false);

            var validator = new CreateSiteValidator(siteRules.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Id, command);
        }

        [Test]
        public void Should_have_error_when_site_name_is_empty()
        {
            var command = new CreateSite
            {
                Id = Guid.NewGuid(),
                Name = ""
            };

            var siteRules = new Mock<ISiteRules>();
            var validator = new CreateSiteValidator(siteRules.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Test]
        public void Should_have_error_when_site_name_is_too_short()
        {
            var command = new CreateSite
            {
                Id = Guid.NewGuid(),
                Name = "My"
            };

            var siteRules = new Mock<ISiteRules>();
            var validator = new CreateSiteValidator(siteRules.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Test]
        public void Should_have_error_when_site_name_is_too_long()
        {
            var name = "";
            for (int i = 0; i < 101; i++) name += i;

            var command = new CreateSite
            {
                Id = Guid.NewGuid(),
                Name = name
            };

            var siteRules = new Mock<ISiteRules>();
            var validator = new CreateSiteValidator(siteRules.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Test]
        public void Should_have_error_when_site_name_already_exists()
        {
            var command = new CreateSite
            {
                Id = Guid.NewGuid(),
                Name = "My Site"
            };

            var siteRules = new Mock<ISiteRules>();
            siteRules.Setup(x => x.IsSiteNameUnique(command.Name)).Returns(false);

            var validator = new CreateSiteValidator(siteRules.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Test]
        public void Should_have_error_when_site_name_is_not_valid()
        {
            var command = new CreateSite
            {
                Id = Guid.NewGuid(),
                Name = "My@Site"
            };

            var siteRules = new Mock<ISiteRules>();
            siteRules.Setup(x => x.IsSiteNameValid(command.Name)).Returns(false);

            var validator = new CreateSiteValidator(siteRules.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }
    }
}
