using Weapsy.Domain.Models.Sites.Commands;
using Weapsy.Domain.Services.Sites.Rules;

namespace Weapsy.Domain.Services.Sites.Validators
{
    public class UpdateSiteValidator : SiteValidatorBase<UpdateSite>
    {
        public UpdateSiteValidator(ISiteRules rules) : base(rules)
        {
            ValidateName();
        }
    }
}
