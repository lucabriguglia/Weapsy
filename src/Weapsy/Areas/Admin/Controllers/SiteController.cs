using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using Weapsy.Domain.Sites;
using Weapsy.Domain.Sites.Commands;
using Weapsy.Infrastructure.Dispatcher;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;
using Weapsy.Reporting.Sites;

namespace Weapsy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SiteController : BaseAdminController
    {
        private readonly ISiteFacade _siteFacade;
        private readonly ICommandSender _commandSender;
        private readonly IMapper _mapper;

        public SiteController(ISiteFacade siteFacade, 
            ICommandSender commandSender, 
            IMapper mapper,
            IContextService contextService)
            : base(contextService)
        {
            _siteFacade = siteFacade;
            _commandSender = commandSender;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Settings");
        }

        public async Task<IActionResult> Settings()
        {
            var model = await Task.Run(() => _siteFacade.GetAdminModel(SiteId));

            if (model == null)
                return NotFound();

            return View("Edit", model);
        }

        public async Task<IActionResult> Update(SiteAdminModel model)
        {
            var command = _mapper.Map<UpdateSiteDetails>(model);
            command.SiteId = SiteId;
            await Task.Run(() => _commandSender.Send<UpdateSiteDetails, Site>(command));
            return new NoContentResult();
        }
    }
}
