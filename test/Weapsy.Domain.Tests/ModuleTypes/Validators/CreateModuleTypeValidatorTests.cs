using System;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.ModuleTypes.Commands;
using Weapsy.Domain.ModuleTypes.Validators;
using Weapsy.Domain.ModuleTypes.Rules;
using Weapsy.Domain.Apps.Rules;

namespace Weapsy.Domain.Tests.ModuleTypes.Validators
{
    [TestFixture]
    public class CreateModuleTypeValidatorTests
    {
        [Test]
        public void Should_have_validation_error_when_module_type_id_already_exists()
        {
            var command = new CreateModuleType
            {
                AppId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "Name",
                Title = "Title",
                Description = "Description"
            };

            var moduleTypeRulesMock = new Mock<IModuleTypeRules>();
            moduleTypeRulesMock.Setup(x => x.IsModuleTypeIdUnique(command.Id)).Returns(false);

            var appRulesMock = new Mock<IAppRules>();

            var validator = new CreateModuleTypeValidator(moduleTypeRulesMock.Object, appRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Id, command);
        }

        [Test]
        public void Should_have_validation_error_when_app_id_is_empty()
        {
            var command = new CreateModuleType
            {
                AppId = Guid.Empty,
                Id = Guid.NewGuid(),
                Name = "Name",
                Title = "Title",
                Description = "Description"
            };

            var moduleTypeRulesMock = new Mock<IModuleTypeRules>();
            var appRulesMock = new Mock<IAppRules>();
            var validator = new CreateModuleTypeValidator(moduleTypeRulesMock.Object, appRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Id, command);
        }

        [Test]
        public void Should_have_validation_error_when_app_does_not_exist()
        {
            var command = new CreateModuleType
            {
                AppId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "Name",
                Title = "Title",
                Description = "Description"
            };

            var moduleTypeRulesMock = new Mock<IModuleTypeRules>();

            var appRulesMock = new Mock<IAppRules>();
            appRulesMock.Setup(x => x.DoesAppExist(command.AppId)).Returns(false);

            var validator = new CreateModuleTypeValidator(moduleTypeRulesMock.Object, appRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.AppId, command);
        }
    }
}
