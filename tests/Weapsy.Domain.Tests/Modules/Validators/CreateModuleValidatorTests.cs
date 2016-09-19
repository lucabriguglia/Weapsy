using System;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Model.Modules.Commands;
using Weapsy.Domain.Model.Modules.Validators;
using Weapsy.Domain.Model.Modules.Rules;
using Weapsy.Domain.Model.Sites.Rules;
using Weapsy.Domain.Model.ModuleTypes.Rules;

namespace Weapsy.Domain.Tests.Modules.Validators
{
    [TestFixture]
    public class CreateModuleValidatorTests
    {
        [Test]
        public void Should_have_error_when_module_id_is_empty()
        {
            var moduleRulesMock = new Mock<IModuleRules>();
            var moduleTypeRules = new Mock<IModuleTypeRules>();
            var siteRulesMock = new Mock<ISiteRules>();

            var validator = new CreateModuleValidator(moduleRulesMock.Object, moduleTypeRules.Object, siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Id, new CreateModule
            {
                SiteId = Guid.NewGuid(),
                ModuleTypeId = Guid.NewGuid(),
                Id = Guid.Empty,
                Title = "Title"
            });
        }

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
                Id = Guid.Empty,
                Title = "Title"
            });
        }

        [Test]
        public void Should_have_validation_error_when_site_id_is_empty()
        {
            var command = new CreateModule
            {
                SiteId = Guid.Empty,
                ModuleTypeId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Title = "Title"
            };

            var moduleRulesMock = new Mock<IModuleRules>();
            var moduleTypeRules = new Mock<IModuleTypeRules>();
            var siteRulesMock = new Mock<ISiteRules>();

            var validator = new CreateModuleValidator(moduleRulesMock.Object, moduleTypeRules.Object, siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.SiteId, command);
        }

        [Test]
        public void Should_have_validation_error_when_site_does_not_exist()
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
            var siteRulesMock = new Mock<ISiteRules>();
            siteRulesMock.Setup(x => x.DoesSiteExist(command.SiteId)).Returns(false);

            var validator = new CreateModuleValidator(moduleRulesMock.Object, moduleTypeRules.Object, siteRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.SiteId, command);
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
