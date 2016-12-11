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
    public class ModuleTypeDetailsValidatorTests
    {
        [Test]
        public void Should_have_error_when_module_type_name_is_empty()
        {
            var moduleTypeRules = new Mock<IModuleTypeRules>();
            var validator = new ModuleTypeDetailsValidator<ModuleTypeDetails>(moduleTypeRules.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, new ModuleTypeDetails
            {
                Id = Guid.NewGuid(),
                Name = string.Empty,
                Title = "Title",
                Description = "Description"
            });
        }

        [Test]
        public void Should_have_error_when_module_type_name_is_too_long()
        {
            var moduleTypeRules = new Mock<IModuleTypeRules>();
            var validator = new ModuleTypeDetailsValidator<ModuleTypeDetails>(moduleTypeRules.Object);

            var name = "";
            for (int i = 0; i < 101; i++) name += i;

            validator.ShouldHaveValidationErrorFor(x => x.Name, new ModuleTypeDetails
            {
                Id = Guid.NewGuid(),
                Name = name,
                Title = "Title",
                Description = "Description"
            });
        }

        [Test]
        public void Should_have_error_when_module_type_name_is_not_valid()
        {
            const string name = "My@ModuleType";

            var moduleTypeRules = new Mock<IModuleTypeRules>();
            moduleTypeRules.Setup(x => x.IsModuleTypeNameValid(name)).Returns(false);

            var validator = new ModuleTypeDetailsValidator<ModuleTypeDetails>(moduleTypeRules.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, new ModuleTypeDetails
            {
                Id = Guid.NewGuid(),
                Name = name,
                Title = "Title",
                Description = "Description"
            });
        }

        [Test]
        public void Should_have_error_when_module_type_name_is_not_unique()
        {
            Guid moduleTypeId = Guid.NewGuid();
            const string name = "My Module Type";

            var moduleTypeRules = new Mock<IModuleTypeRules>();
            moduleTypeRules.Setup(x => x.IsModuleTypeNameUnique(name, moduleTypeId)).Returns(false);

            var validator = new ModuleTypeDetailsValidator<ModuleTypeDetails>(moduleTypeRules.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, new ModuleTypeDetails
            {
                Id = moduleTypeId,
                Name = name
            });
        }

        [Test]
        public void Should_have_error_when_module_type_title_is_too_long()
        {
            var moduleTypeRules = new Mock<IModuleTypeRules>();
            var validator = new ModuleTypeDetailsValidator<ModuleTypeDetails>(moduleTypeRules.Object);

            var title = "";
            for (int i = 0; i < 251; i++) title += i;

            validator.ShouldHaveValidationErrorFor(x => x.Title, new ModuleTypeDetails
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Title = title,
                Description = string.Empty
            });
        }

        [Test]
        public void Should_have_error_when_module_type_description_is_too_long()
        {
            var moduleTypeRules = new Mock<IModuleTypeRules>();
            var validator = new ModuleTypeDetailsValidator<ModuleTypeDetails>(moduleTypeRules.Object);

            var description = "";
            for (int i = 0; i < 501; i++) description += i;

            validator.ShouldHaveValidationErrorFor(x => x.Description, new ModuleTypeDetails
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Title = string.Empty,
                Description = description
            });
        }

        [Test]
        public void Should_have_error_when_module_type_edit_url_is_too_long()
        {
            var moduleTypeRules = new Mock<IModuleTypeRules>();
            var validator = new ModuleTypeDetailsValidator<ModuleTypeDetails>(moduleTypeRules.Object);

            var editUrl = "";
            for (int i = 0; i < 101; i++) editUrl += i;

            validator.ShouldHaveValidationErrorFor(x => x.EditUrl, new ModuleTypeDetails
            {
                EditUrl = editUrl
            });
        }

        [Test]
        public void Should_have_error_when_module_type_view_name_is_empty()
        {
            var moduleTypeRules = new Mock<IModuleTypeRules>();
            var validator = new ModuleTypeDetailsValidator<ModuleTypeDetails>(moduleTypeRules.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Name, new ModuleTypeDetails
            {
                ViewName = string.Empty
            });
        }

        [Test]
        public void Should_have_error_when_module_type_view_name_is_too_long()
        {
            var moduleTypeRules = new Mock<IModuleTypeRules>();
            var validator = new ModuleTypeDetailsValidator<ModuleTypeDetails>(moduleTypeRules.Object);

            var viewName = "";
            for (int i = 0; i < 101; i++) viewName += i;

            validator.ShouldHaveValidationErrorFor(x => x.Name, new ModuleTypeDetails
            {
                Name = viewName
            });
        }

        [Ignore("Business logic to be reviewed.")]
        [Test]
        public void Should_have_error_when_module_type_view_name_is_not_unique()
        {
            Guid moduleTypeId = Guid.NewGuid();
            const string viewName = "MyModuleType";

            var moduleTypeRules = new Mock<IModuleTypeRules>();
            moduleTypeRules.Setup(x => x.IsModuleTypeViewComponentNameUnique(viewName, moduleTypeId)).Returns(false);

            var validator = new ModuleTypeDetailsValidator<ModuleTypeDetails>(moduleTypeRules.Object);

            validator.ShouldHaveValidationErrorFor(x => x.ViewName, new ModuleTypeDetails
            {
                Id = moduleTypeId,
                Name = viewName,
                ViewType = Domain.ModuleTypes.ViewType.ViewComponent
            });
        }

        [Test]
        public void Should_have_error_when_module_type_view_type_is_empty()
        {
            var moduleTypeRules = new Mock<IModuleTypeRules>();
            var validator = new ModuleTypeDetailsValidator<ModuleTypeDetails>(moduleTypeRules.Object);

            validator.ShouldHaveValidationErrorFor(x => x.ViewType, new ModuleTypeDetails {});
        }

        [Test]
        public void Should_have_error_when_module_type_edit_type_is_empty()
        {
            var moduleTypeRules = new Mock<IModuleTypeRules>();
            var validator = new ModuleTypeDetailsValidator<ModuleTypeDetails>(moduleTypeRules.Object);

            validator.ShouldHaveValidationErrorFor(x => x.EditType, new ModuleTypeDetails { });
        }
    }
}
