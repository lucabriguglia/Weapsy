using Microsoft.AspNetCore.Mvc;
using System;
using Weapsy.Mvc.Context;

namespace Weapsy.Mvc.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly IContextService _contextService;

        public BaseController(IContextService contextService)
        {
            _contextService = contextService;
        }

        public SiteInfo SiteInfo => _contextService.GetCurrentSiteInfo();
        public Guid SiteId => SiteInfo.Id;
    }
}
