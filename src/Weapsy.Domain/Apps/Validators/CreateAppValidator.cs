using System;
using FluentValidation;
using Weapsy.Domain.Model.Apps.Commands;
using Weapsy.Domain.Model.Apps.Rules;

namespace Weapsy.Domain.Model.Apps.Validators
{
    public class CreateAppValidator : AppDetailsValidator<CreateApp>
    {
        private readonly IAppRules _appRules;

        public CreateAppValidator(IAppRules appRules) : base(appRules)
        {
            _appRules = appRules;

            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Id is required.")
                .Must(HaveUniqueId).WithMessage("An app with the same id already exists.");
        }

        private bool HaveUniqueId(Guid id)
        {
            return _appRules.IsAppIdUnique(id);
        }
    }
}
