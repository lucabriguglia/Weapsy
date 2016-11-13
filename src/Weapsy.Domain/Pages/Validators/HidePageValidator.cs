using System;
using FluentValidation;
using Weapsy.Domain.Pages.Commands;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Pages.Validators
{
    public class HidePageValidator : BaseSiteValidator<HidePage>
    {
        private readonly ISiteRules _siteRules;

        public HidePageValidator(ISiteRules siteRules)
            : base(siteRules)
        {
            _siteRules = siteRules;

            RuleFor(c => c.Id)
                .Must(NotBeInUseAsHomePage)
                    .WithMessage("Page is used as Home Page and cannot be hidden.");
        }

        private bool NotBeInUseAsHomePage(HidePage cmd, Guid pageId)
        {
            return !_siteRules.IsPageSetAsHomePage(cmd.SiteId, pageId);
        }
    }
}