using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using Weapsy.Domain.Sites;
using Weapsy.Domain.Sites.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Queries;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;
using Weapsy.Reporting.Sites;
using Weapsy.Reporting.Sites.Queries;

namespace Weapsy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SiteController : BaseAdminController
    {
        private readonly ICommandSender _commandSender;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly IMapper _mapper;

        public SiteController(ICommandSender commandSender,
            IQueryDispatcher queryDispatcher,
            IMapper mapper,
            IContextService contextService)
            : base(contextService)
        {
            _commandSender = commandSender;
            _mapper = mapper;
            _queryDispatcher = queryDispatcher;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Settings");
        }

        public async Task<IActionResult> Settings()
        {
            var model = await _queryDispatcher.DispatchAsync<GetAdminModel, SiteAdminModel>(new GetAdminModel { Id = SiteId });

            if (model == null)
                return NotFound();

            return View("Edit", model);
        }

        public IActionResult Update(SiteAdminModel model)
        {
            var command = _mapper.Map<UpdateSiteDetails>(model);
            command.SiteId = SiteId;
            _commandSender.Send<UpdateSiteDetails, Site>(command);
            return new NoContentResult();
        }
    }
}
