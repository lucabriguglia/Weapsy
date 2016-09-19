using System;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Model.Modules.Commands;
using Weapsy.Domain.Model.Modules.Rules;
using Weapsy.Domain.Model.Modules.Validators;
using Weapsy.Domain.Model.Sites.Rules;

namespace Weapsy.Domain.Tests.Modules.Validators
{
    [TestFixture]
    public class DeleteModuleValidatorTests
    {
        [Test]
        public void Should_have_validation_error_when_site_id_is_empty()
        {
            var command = new DeleteModule
            {
                SiteId = Guid.Empty,
                Id = Guid.NewGuid()
            };

            var moduleRulesMock = new Mock<IModuleRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var validator = new DeleteModuleValidator(siteRulesMock.Object, moduleRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.SiteId, command);
        }

        [Test]
        public void Should_have_validation_error_when_site_does_not_exist()
        {
            var command = new DeleteModule
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid()
            };

            var moduleRulesMock = new Mock<IModuleRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            siteRulesMock.Setup(x => x.DoesSiteExist(command.SiteId)).Returns(false);

            var validator = new DeleteModuleValidator(siteRulesMock.Object, moduleRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.SiteId, command);
        }

        [Test]
        public void Should_have_error_when_modue_type_id_is_empty()
        {
            var moduleRulesMock = new Mock<IModuleRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var validator = new DeleteModuleValidator(siteRulesMock.Object, moduleRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Id, new DeleteModule
            {
                Id = Guid.Empty
            });
        }

        [Test]
        public void Should_have_error_when_modue_type_is_in_use()
        {
            Guid moduleId = Guid.NewGuid();

            var moduleRulesMock = new Mock<IModuleRules>();
            moduleRulesMock.Setup(x => x.IsModuleInUse(moduleId)).Returns(true);

            var siteRulesMock = new Mock<ISiteRules>();
            var validator = new DeleteModuleValidator(siteRulesMock.Object, moduleRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Id, new DeleteModule
            {
                Id = moduleId
            });
        }
    }
}
