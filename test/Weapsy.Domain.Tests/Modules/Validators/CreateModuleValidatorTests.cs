using System;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Modules.Commands;
using Weapsy.Domain.Modules.Validators;
using Weapsy.Domain.Modules.Rules;
using Weapsy.Domain.Sites.Rules;
using Weapsy.Domain.ModuleTypes.Rules;

namespace Weapsy.Domain.Tests.Modules.Validators
{
    [TestFixture]
    public class CreateModuleValidatorTests
    {
        [Test]
        public void Should_have_error_when_module_id_already_exists()
        {
            Guid id = Guid.NewGuid();

            var moduleRulesMock = new Mock<IModuleRules>();
            moduleRulesMock.Setup(x => x.IsModuleIdUnique(id)).Returns(false);
            var moduleTypeRules = new Mock<IModuleTypeRules>();
            var siteRulesMock = new Mock<ISiteRules>();

            var validator = new CreateModuleValidator(moduleRulesMock.Object, moduleTypeRules.Object, siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Id, new CreateModule
            {
                SiteId = Guid.NewGuid(),
                ModuleTypeId = id,
                Id = id,
                Title = "Title"
            });
        }

        [Test]
        public void Should_have_validation_error_when_module_type_id_is_empty()
        {
            var command = new CreateModule
            {
                SiteId = Guid.NewGuid(),
                ModuleTypeId = Guid.Empty,
                Id = Guid.NewGuid(),
                Title = "Title"
            };

            var moduleRulesMock = new Mock<IModuleRules>();
            var moduleTypeRules = new Mock<IModuleTypeRules>();
            var siteRulesMock = new Mock<ISiteRules>();

            var validator = new CreateModuleValidator(moduleRulesMock.Object, moduleTypeRules.Object, siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.ModuleTypeId, command);
        }

        [Test]
        public void Should_have_validation_error_when_module_type_does_not_exist()
        {
            var command = new CreateModule
            {
                SiteId = Guid.NewGuid(),
                ModuleTypeId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Title = "Title"
            };

            var moduleRulesMock = new Mock<IModuleRules>();
            var moduleTypeRules = new Mock<IModuleTypeRules>();
            moduleTypeRules.Setup(x => x.DoesModuleTypeExist(command.SiteId)).Returns(false);
            var siteRulesMock = new Mock<ISiteRules>();           

            var validator = new CreateModuleValidator(moduleRulesMock.Object, moduleTypeRules.Object, siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.ModuleTypeId, command);
        }

        [Test]
        public void Should_have_error_when_module_title_is_empty()
        {
            var moduleRulesMock = new Mock<IModuleRules>();
            var moduleTypeRules = new Mock<IModuleTypeRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var validator = new CreateModuleValidator(moduleRulesMock.Object, moduleTypeRules.Object, siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Title, new CreateModule
            {
                SiteId = Guid.NewGuid(),
                ModuleTypeId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Title = ""
            });
        }

        [Test]
        public void Should_have_error_when_module_title_is_too_long()
        {
            var moduleRulesMock = new Mock<IModuleRules>();
            var moduleTypeRules = new Mock<IModuleTypeRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var validator = new CreateModuleValidator(moduleRulesMock.Object, moduleTypeRules.Object, siteRulesMock.Object);

            var title = "";
            for (int i = 0; i < 101; i++) title += i;

            validator.ShouldHaveValidationErrorFor(x => x.Title, new CreateModule
            {
                SiteId = Guid.NewGuid(),
                ModuleTypeId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Title = title
            });
        }
    }
}
