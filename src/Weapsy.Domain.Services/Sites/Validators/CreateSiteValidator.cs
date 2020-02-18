using Weapsy.Domain.Models.Sites.Commands;
using Weapsy.Domain.Services.Sites.Rules;

namespace Weapsy.Domain.Services.Sites.Validators
{
    public class CreateSiteValidator : SiteValidatorBase<CreateSite>
    {
        public CreateSiteValidator(ISiteRules rules) : base(rules)
        {
            ValidateName();
        }
    }
}
