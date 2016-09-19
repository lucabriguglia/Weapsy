using System;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.ModuleTypes.Commands;
using Weapsy.Domain.ModuleTypes.Rules;
using Weapsy.Domain.ModuleTypes.Validators;

namespace Weapsy.Domain.Tests.ModuleTypes.Validators
{
    [TestFixture]
    public class DeleteModuleTypeValidatorTests
    {
        [Test]
        public void Should_have_error_when_modue_type_id_is_empty()
        {
            var moduleTypeRules = new Mock<IModuleTypeRules>();
            var validator = new DeleteModuleTypeValidator(moduleTypeRules.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Id, new DeleteModuleType
            {
                Id = Guid.Empty
            });
        }

        [Test]
        public void Should_have_error_when_modue_type_is_in_use()
        {
            Guid moduleTypeId = Guid.NewGuid();

            var moduleTypeRules = new Mock<IModuleTypeRules>();
            moduleTypeRules.Setup(x => x.IsModuleTypeInUse(moduleTypeId)).Returns(true);

            var validator = new DeleteModuleTypeValidator(moduleTypeRules.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Id, new DeleteModuleType
            {
                Id = moduleTypeId
            });
        }
    }
}
