using System;
using FluentValidation;
using Weapsy.Domain.Apps.Rules;

namespace Weapsy.Domain.Apps.Commands.Handlers.Validators
{
    public class CreateAppValidator : BaseValidator<CreateApp>
    {
        private readonly IAppRules _appRules;

        public CreateAppValidator(IAppRules appRules) : base(appRules)
        {
            _appRules = appRules;

            ValidateId();
            ValidateName();
            ValidateDescription();
            ValidateFolder();
        }

        private void ValidateId()
        {
            RuleFor(c => c.Id)
                .Must(HaveUniqueId).WithMessage("An app with the same id already exists.")
                .When(x => x.Id != Guid.Empty);
        }

        private bool HaveUniqueId(Guid id)
        {
            return _appRules.IsAppIdUnique(id);
        }
    }
}
