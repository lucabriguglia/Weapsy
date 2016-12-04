using Microsoft.AspNetCore.Mvc;
using System;
using Weapsy.Mvc.Context;
using Weapsy.Reporting.Languages;
using Weapsy.Reporting.Sites;

namespace Weapsy.Mvc.Components
{
    public abstract class BaseViewComponent : ViewComponent
    {
        private readonly IContextService _contextService;

        protected BaseViewComponent(IContextService contextService)
        {
            _contextService = contextService;
        }

        public SiteInfo SiteInfo => _contextService.GetCurrentSiteInfo();
        public LanguageInfo LanguageInfo => _contextService.GetCurrentLanguageInfo();
        public Guid SiteId => SiteInfo.Id;
    }
}
