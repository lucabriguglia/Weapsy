using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Cqrs;
using Weapsy.Domain.Sites;
using Weapsy.Domain.Sites.Commands;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;
using Weapsy.Reporting.Sites;
using Weapsy.Reporting.Sites.Queries;

namespace Weapsy.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SiteController : BaseAdminController
    {
        private readonly IDispatcher _dispatcher;
        private readonly IMapper _mapper;

        public SiteController(IDispatcher dispatcher,
            IMapper mapper,
            IContextService contextService)
            : base(contextService)
        {
            _dispatcher = dispatcher;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Settings");
        }

        public async Task<IActionResult> Settings()
        {
            var model = await _dispatcher.GetResultAsync<GetAdminModel, SiteAdminModel>(new GetAdminModel { Id = SiteId });

            if (model == null)
                return NotFound();

            return View("Edit", model);
        }

        public IActionResult Update(SiteAdminModel model)
        {
            var command = _mapper.Map<UpdateSiteDetails>(model);
            command.SiteId = SiteId;
            _dispatcher.SendAndPublish<UpdateSiteDetails, Site>(command);
            return new NoContentResult();
        }
    }
}
