using Weapsy.Domain.Model.Apps.Commands;
using Weapsy.Domain.Model.Apps.Rules;

namespace Weapsy.Domain.Model.Apps.Validators
{
    public class UpdateAppDetailsValidator : AppDetailsValidator<UpdateAppDetails>
    {
        public UpdateAppDetailsValidator(IAppRules appRules) : base(appRules)
        {
        }
    }
}
