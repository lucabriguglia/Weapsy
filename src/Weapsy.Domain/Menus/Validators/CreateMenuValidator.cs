using System;
using FluentValidation;
using Weapsy.Domain.Menus.Commands;
using Weapsy.Domain.Menus.Rules;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Menus.Validators
{
    public class CreateMenuValidator : BaseSiteValidator<CreateMenu>
    {
        private readonly IMenuRules _menuRules;

        public CreateMenuValidator(IMenuRules menuRules, ISiteRules siteRules)
            : base(siteRules)
        {
            _menuRules = menuRules;

            RuleFor(c => c.Id)
                .Must(HaveUniqueId).WithMessage("A menu with the same id already exists.")
                .When(x => x.Id != Guid.Empty);

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Menu name is required.")
                .Length(3, 100).WithMessage("Menu name length must be between 3 and 100 characters.")
                .Must(HaveValidName).WithMessage("Menu name is not valid. Enter only letters, numbers, underscores and hyphens.")
                .Must(HaveUniqueName).WithMessage("A menu with the same name already exists.");
        }

        private bool HaveUniqueId(Guid id)
        {
            return _menuRules.IsMenuIdUnique(id);
        }

        private bool HaveUniqueName(CreateMenu cmd, string name)
        {
            return _menuRules.IsMenuNameUnique(cmd.SiteId, name);
        }

        private bool HaveValidName(string name)
        {
            return _menuRules.IsMenuNameValid(name);
        }
    }
}
