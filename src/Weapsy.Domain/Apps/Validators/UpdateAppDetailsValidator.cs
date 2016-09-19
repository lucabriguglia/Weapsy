using Weapsy.Domain.Apps.Commands;
using Weapsy.Domain.Apps.Rules;

namespace Weapsy.Domain.Apps.Validators
{
    public class UpdateAppDetailsValidator : AppDetailsValidator<UpdateAppDetails>
    {
        public UpdateAppDetailsValidator(IAppRules appRules) : base(appRules)
        {
        }
    }
}
