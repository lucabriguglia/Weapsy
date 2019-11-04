using Weapsy.Domain.Apps.Rules;

namespace Weapsy.Domain.Apps.Commands.Handlers.Validators
{
    public class UpdateAppDetailsValidator : BaseValidator<UpdateAppDetails>
    {
        public UpdateAppDetailsValidator(IAppRules appRules) : base(appRules)
        {
            ValidateName();
            ValidateDescription();
            ValidateFolder();
        }
    }
}
