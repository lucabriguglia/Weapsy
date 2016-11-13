using FluentValidation;
using System;
using Weapsy.Domain.Pages.Commands;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Pages.Validators
{
    public class DeletePageValidator : BaseSiteValidator<DeletePage>
    {
        private readonly ISiteRules _siteRules;

        public DeletePageValidator(ISiteRules siteRules)
            : base(siteRules)
        {
            _siteRules = siteRules;

            RuleFor(c => c.Id)
                .Must(NotBeInUseAsHomePage)
                    .WithMessage("Page is used as Home Page and cannot be deleted.");
        }

        private bool NotBeInUseAsHomePage(DeletePage cmd, Guid pageId)
        {
            return !_siteRules.IsPageSetAsHomePage(cmd.SiteId, pageId);
        }
    }
}