using FluentValidation;
using Weapsy.Domain.Apps.Commands;
using Weapsy.Domain.Apps.Rules;

namespace Weapsy.Domain.Apps.Validators
{
    public class AppDetailsValidator<T> : AbstractValidator<T> where T : AppDetails
    {
        private readonly IAppRules _appRules;

        public AppDetailsValidator(IAppRules appRules)
        {
            _appRules = appRules;

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("App name is required.")
                .Length(1, 100).WithMessage("App name length must be between 1 and 100 characters.")
                .Must(HaveUniqueName).WithMessage("A app with the same name already exists.");

            RuleFor(c => c.Description)
                .Length(1, 4000).WithMessage("Culture name length must be between 1 and 4000 characters.")
                .When(c => !string.IsNullOrWhiteSpace(c.Description));

            RuleFor(c => c.Folder)
                .NotEmpty().WithMessage("App folder is required.")
                .Length(1, 100).WithMessage("App url length must be between 1 and 100 characters.")
                .Must(HaveValidFolder).WithMessage("App folder is not valid. Enter only letters and 1 hyphen.")
                .Must(HaveUniqueFolder).WithMessage("A app with the same folder already exists.");
        }

        private bool HaveUniqueName(AppDetails cmd, string name)
        {
            return _appRules.IsAppNameUnique(name, cmd.Id);
        }

        private bool HaveValidFolder(string folder)
        {
            return _appRules.IsAppFolderValid(folder);
        }

        private bool HaveUniqueFolder(AppDetails cmd, string folder)
        {
            return _appRules.IsAppFolderUnique(folder, cmd.Id);
        }
    }
}
