using Microsoft.AspNetCore.Mvc;
using System;
using Weapsy.Mvc.Context;
using Weapsy.Reporting.Sites;

namespace Weapsy.Mvc.Components
{
    public abstract class BaseViewComponent : ViewComponent
    {
        private readonly IContextService _contextService;

        public BaseViewComponent(IContextService contextService)
        {
            _contextService = contextService;
        }

        public SiteInfo SiteInfo => _contextService.GetCurrentSiteInfo();
        public Guid SiteId => SiteInfo.Id;
    }
}
