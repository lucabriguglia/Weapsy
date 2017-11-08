using Weapsy.Domain.Languages.Commands;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Languages.Validators
{
    public class DeleteLanguageValidator : BaseSiteValidator<DeleteLanguageCommand>
    {
        public DeleteLanguageValidator(ISiteRules siteRules)
            : base(siteRules)
        {
        }
    }
}