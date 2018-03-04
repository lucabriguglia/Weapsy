using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Cqrs;
using Weapsy.Domain.Pages;
using Weapsy.Domain.Pages.Commands;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;
using Weapsy.Reporting.Pages;
using Weapsy.Reporting.Pages.Queries;

namespace Weapsy.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PageController : BaseAdminController
    {
        private readonly IDispatcher _dispatcher;
        private readonly IMapper _mapper;

        public PageController(IDispatcher dispatcher,
            IMapper mapper,
            IContextService contextService)
            : base(contextService)
        {
            _dispatcher = dispatcher;
            _mapper = mapper;            
        }

        public async Task<IActionResult> Index()
        {
            var model = await _dispatcher.GetResultAsync<GetAllForAdmin, IEnumerable<PageAdminListModel>>(new GetAllForAdmin
            {
                SiteId = SiteId
            });
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = await _dispatcher.GetResultAsync<GetDefaultForAdmin, PageAdminModel>(new GetDefaultForAdmin
            {
                SiteId = SiteId
            });

            return View(model);
        }

        public IActionResult Save(PageAdminModel model)
        {
            var command = _mapper.Map<CreatePage>(model);
            command.SiteId = SiteId;
            command.Id = Guid.NewGuid();
            command.PagePermissions = model.PagePermissions.ToDomain();
            command.MenuIds = model.Menus.ToCommand();
            _dispatcher.SendAndPublish<CreatePage, Page>(command);
            return new NoContentResult();
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await _dispatcher.GetResultAsync<GetForAdmin, PageAdminModel>(new GetForAdmin
            {
                SiteId = SiteId,
                Id = id
            });

            if (model == null)
                return NotFound();

            return View(model);
        }

        public IActionResult Update(PageAdminModel model)
        {
            var command = _mapper.Map<UpdatePageDetails>(model);
            command.SiteId = SiteId;
            command.PagePermissions = model.PagePermissions.ToDomain();
            _dispatcher.SendAndPublish<UpdatePageDetails, Page>(command);
            return new NoContentResult();
        }

        public async Task<IActionResult> EditModule(Guid pageId, Guid pageModuleId)
        {
            var model = await _dispatcher.GetResultAsync<GetPageModuleAdminModel, PageModuleAdminModel>(new GetPageModuleAdminModel
            {
                SiteId = SiteId,
                PageId = pageId,
                PageModuleId = pageModuleId
            });

            if (model == null)
                return NotFound();

            return View(model);
        }

        public IActionResult UpdateModule(PageModuleAdminModel model)
        {
            var command = _mapper.Map<UpdatePageModuleDetails>(model);
            command.SiteId = SiteId;
            command.PageModulePermissions = model.PageModulePermissions.ToDomain();
            _dispatcher.SendAndPublish<UpdatePageModuleDetails, Page>(command);
            return new NoContentResult();
        }
    }
}
