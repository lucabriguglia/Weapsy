using FluentValidation;
using Weapsy.Domain.ModuleTypes.Commands;
using Weapsy.Domain.ModuleTypes.Rules;

namespace Weapsy.Domain.ModuleTypes.Validators
{
    public class ModuleTypeDetailsValidator<T> : AbstractValidator<T> where T : ModuleTypeDetails
    {
        private readonly IModuleTypeRules _moduleTypeRules;

        public ModuleTypeDetailsValidator(IModuleTypeRules moduleTypeRules)
        {
            _moduleTypeRules = moduleTypeRules;

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Module type name is required.")
                .Length(1, 100).WithMessage("Module type name length must be between 1 and 100 characters.")
                .Must(HaveValidName).WithMessage("Module type name is not valid. Enter only letters, numbers, underscores and hyphens.")
                .Must(HaveUniqueName).WithMessage("A module type with the same name already exists.");

            RuleFor(c => c.Title)
                .Length(1, 250).WithMessage("Title cannot have more than 250 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Title));

            RuleFor(c => c.Description)
                .Length(1, 500).WithMessage("Description cannot have more than 500 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Description));

            RuleFor(c => c.ViewType)
                .NotEmpty().WithMessage("View type is required.");

            RuleFor(c => c.ViewName)
                .NotEmpty().WithMessage("View name is required.")
                .Length(1, 100).WithMessage("View name length must be between 1 and 100 characters.")
                .Must(HaveUniqueViewComponentName).WithMessage("A module type with the same view component name already exists.")
                .When(c => c.ViewType == ViewType.ViewComponent);

            RuleFor(c => c.EditType)
                .NotEmpty().WithMessage("Edit Type is required.");

            RuleFor(c => c.EditUrl)
                .Length(1, 100).WithMessage("Edit Url length must be between 1 and 100 characters.")
                .When(c => !string.IsNullOrWhiteSpace(c.EditUrl));
        }

        private bool HaveUniqueName(ModuleTypeDetails cmd, string name)
        {
            return _moduleTypeRules.IsModuleTypeNameUnique(name, cmd.Id);
        }

        private bool HaveValidName(string name)
        {
            return _moduleTypeRules.IsModuleTypeNameValid(name);
        }

        private bool HaveUniqueViewComponentName(ModuleTypeDetails cmd, string viewComponentName)
        {
            return _moduleTypeRules.IsModuleTypeViewComponentNameUnique(viewComponentName, cmd.Id);
        }
    }
}
