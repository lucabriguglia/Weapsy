using System;
using FluentValidation;
using Weapsy.Domain.Model.Menus.Commands;
using Weapsy.Domain.Model.Menus.Rules;
using Weapsy.Domain.Model.Sites.Rules;

namespace Weapsy.Domain.Model.Menus.Validators
{
    public class CreateMenuValidator : AbstractValidator<CreateMenu>
    {
        private readonly IMenuRules _menuRules;
        private readonly ISiteRules _siteRules;

        public CreateMenuValidator(IMenuRules menuRules, ISiteRules siteRules)
        {
            _menuRules = menuRules;
            _siteRules = siteRules;

            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Id is required.")
                .Must(HaveUniqueId).WithMessage("A menu with the same id already exists.");

            RuleFor(c => c.SiteId)
                .NotEmpty().WithMessage("Site id is required.")
                .Must(BeAnExistingSite).WithMessage("Site does not exist.");

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

        private bool BeAnExistingSite(Guid siteId)
        {
            return _siteRules.DoesSiteExist(siteId);
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
