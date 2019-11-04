using FluentValidation;
using Weapsy.Domain.Apps.Rules;

namespace Weapsy.Domain.Apps.Commands.Handlers.Validators
{
    public class BaseValidator<T> : AbstractValidator<T> where T : AppDetails
    {
        private readonly IAppRules _appRules;

        public BaseValidator(IAppRules appRules)
        {
            _appRules = appRules;
        }

        protected void ValidateName()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("App name is required.")
                .Length(1, 100).WithMessage("App name length must be between 1 and 100 characters.")
                .Must(HaveUniqueName).WithMessage("A app with the same name already exists.");
        }

        protected void ValidateDescription()
        {
            RuleFor(c => c.Description)
                .Length(1, 4000).WithMessage("Culture name length must be between 1 and 4000 characters.")
                .When(c => !string.IsNullOrWhiteSpace(c.Description));
        }

        protected void ValidateFolder()
        {
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
