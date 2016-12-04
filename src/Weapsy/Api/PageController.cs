using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Weapsy.Infrastructure.Dispatcher;
using Weapsy.Domain.Pages;
using Weapsy.Domain.Pages.Commands;
using Weapsy.Domain.Pages.Rules;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;
using Weapsy.Reporting.Pages;
using Weapsy.Services.Identity;

namespace Weapsy.Api
{
    [Route("api/[controller]")]
    public class PageController : BaseAdminController
    {
        private readonly ICommandSender _commandSender;
        private readonly IPageFacade _pageFacade;        
        private readonly IPageRules _pageRules;
        private readonly IPageRepository _pageRepository;
        private readonly IRoleService _roleService;

        public PageController(ICommandSender commandSender,
            IPageFacade pageFacade,            
            IPageRules pageRules,
            IPageRepository pageRepository,
            IRoleService roleService,
            IContextService contextService)
            : base(contextService)
        {
            _pageFacade = pageFacade;
            _commandSender = commandSender;
            _pageRules = pageRules;
            _pageRepository = pageRepository;
            _roleService = roleService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreatePage model)
        {
            model.SiteId = SiteId;
            _commandSender.Send<CreatePage, Page>(model);
            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/update")]
        public IActionResult UpdateDetails([FromBody] UpdatePageDetails model)
        {
            model.SiteId = SiteId;
             _commandSender.Send<UpdatePageDetails, Page>(model);
            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/add-module")]
        public async Task<IActionResult> AddModule([FromBody] AddModule model)
        {
            model.SiteId = SiteId;
            var defaultViewRoleIds = await _roleService.GetDefaultModuleViewPermissionRoleIdsAsync();
            var defaultEditRoleIds = await _roleService.GetDefaultModuleEditPermissionRoleIdsAsync();
            model.SetPageModulePermissions(defaultViewRoleIds, defaultEditRoleIds);
            await Task.Run(() => _commandSender.Send<AddModule, Page>(model));
            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/remove-module")]
        public IActionResult RemoveModule([FromBody] RemoveModule model)
        {
            model.SiteId = SiteId;
            _commandSender.Send<RemoveModule, Page>(model);
            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/reorder-modules")]
        public IActionResult ReorderPageModules([FromBody] ReorderPageModules model)
        {
            model.SiteId = SiteId;
            _commandSender.Send<ReorderPageModules, Page>(model);
            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/set-permissions")]
        public IActionResult SetPermissions([FromBody] SetPagePermissions model)
        {
            model.SiteId = SiteId;
            _commandSender.Send<SetPagePermissions, Page>(model);
            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/set-module-permissions")]
        public IActionResult SetModulePermissions([FromBody] SetPageModulePermissions model)
        {
            model.SiteId = SiteId;
            _commandSender.Send<SetPageModulePermissions, Page>(model);
            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/activate")]
        public IActionResult Activate(Guid id)
        {
            _commandSender.Send<ActivatePage, Page>(new ActivatePage
            {
                SiteId = SiteId,
                Id = id
            });
            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/hide")]
        public IActionResult Hide(Guid id)
        {
            _commandSender.Send<HidePage, Page>(new HidePage
            {
                SiteId = SiteId,
                Id = id
            });
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _commandSender.Send<DeletePage, Page>(new DeletePage
            {
                SiteId = SiteId,
                Id = id
            });
            return new NoContentResult();
        }

        [HttpGet("{name}")]
        [Route("isPageNameUnique")]
        public IActionResult IsPageNameUnique(string name)
        {
            var isPageNameUnique = _pageRules.IsPageNameUnique(SiteId, name);
            return Ok(isPageNameUnique);
        }

        [HttpGet("{name}")]
        [Route("isPageNameValid")]
        public IActionResult IsPageNameValid(string name)
        {
            var isPageNameValid = _pageRules.IsPageNameValid(name);
            return Ok(isPageNameValid);
        }

        [HttpGet("{url}")]
        [Route("isPageSlugUnique")]
        public IActionResult IsPageSlugUnique(string url)
        {
            var isPageSlugUnique = _pageRules.IsSlugUnique(SiteId, url);
            return Ok(isPageSlugUnique);
        }

        [HttpGet("{url}")]
        [Route("isPageUrlValid")]
        public IActionResult IsPageUrlValid(string url)
        {
            var isPageUrlValid = _pageRules.IsPageUrlValid(url);
            return Ok(isPageUrlValid);
        }

        [HttpGet("{url}")]
        [Route("isPageUrlReserved")]
        public IActionResult IsPageUrlReserved(string url)
        {
            var isPageUrlReserved = _pageRules.IsPageUrlReserved(url);
            return Ok(isPageUrlReserved);
        }

        [HttpGet]
        [Route("{id}/view")]
        public IActionResult ViewById(Guid id)
        {
            var model = _pageFacade.GetPageInfo(SiteId, id);
            if (model == null)
                return NotFound();
            return Ok(model);
        }

        [HttpGet]
        [Route("{name}/view")]
        public IActionResult ViewByName(string name)
        {
            var model = _pageFacade.GetPageInfo(SiteId, name);
            if (model == null)
                return NotFound();
            return Ok(model);
        }

        [HttpGet]
        [Route("admin-list")]
        public IActionResult AdminList()
        {
            var model = _pageFacade.GetAllForAdmin(SiteId);
            return Ok(model);
        }

        [HttpGet]
        [Route("{id}/admin-edit")]
        public IActionResult AdminEdit(Guid id)
        {
            var model = _pageFacade.GetAdminModel(SiteId, id);
            if (model == null)
                return NotFound();
            return Ok(model);
        }
    }
}
