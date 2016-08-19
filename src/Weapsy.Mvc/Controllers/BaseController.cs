using Microsoft.AspNetCore.Mvc;
using System;
using Weapsy.Mvc.Context;
using Weapsy.Reporting.Sites;

namespace Weapsy.Mvc.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly IContextService _contextService;

        public BaseController(IContextService contextService)
        {
            _contextService = contextService;
        }

        public SiteInfo SiteInfo => _contextService.GetCurrentContextInfo().Site;
        public Guid SiteId => SiteInfo.Id;
    }
}
