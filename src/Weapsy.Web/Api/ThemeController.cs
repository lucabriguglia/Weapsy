using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Cqrs;
using Weapsy.Domain.Themes;
using Weapsy.Domain.Themes.Commands;
using Weapsy.Domain.Themes.Rules;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;
using Weapsy.Reporting.Themes;
using Weapsy.Reporting.Themes.Queries;

namespace Weapsy.Web.Api
{
    [Route("api/[controller]")]
    public class ThemeController : BaseAdminController
    {
        private readonly IDispatcher _dispatcher;
        private readonly IThemeRules _themeRules;

        public ThemeController(IDispatcher dispatcher,          
            IThemeRules themeRules,
            IContextService contextService)
            : base(contextService)
        {
            _dispatcher = dispatcher;
            _themeRules = themeRules;
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
        public IActionResult Post([FromBody] CreateTheme model)
        {
            model.Id = Guid.NewGuid();
            _dispatcher.SendAndPublish<CreateTheme, Theme>(model);
            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/update")]
        public IActionResult UpdateDetails([FromBody] UpdateThemeDetails model)
        {
            _dispatcher.SendAndPublish<UpdateThemeDetails, Theme>(model);
            return new NoContentResult();
        }

        [HttpPut]
        [Route("reorder")]
        public IActionResult ReorderThemes([FromBody] List<Guid> model)
        {
            for (var i = 0; i < model.Count; i++)
            {
                _dispatcher.SendAndPublish<ReorderTheme, Theme>(new ReorderTheme
                {
                    AggregateRootId = model[i],
                    SiteId = SiteId,
                    Order = i + 1
                });
            }
            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/activate")]
        public IActionResult Activate(Guid id)
        {
            _dispatcher.SendAndPublish<ActivateTheme, Theme>(new ActivateTheme { Id = id });
            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/hide")]
        public IActionResult Hide(Guid id)
        {
            _dispatcher.SendAndPublish<HideTheme, Theme>(new HideTheme { Id = id });
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _dispatcher.SendAndPublish<DeleteTheme, Theme>(new DeleteTheme { Id = id });
            return new NoContentResult();
        }

        [HttpGet("{name}")]
        [Route("isThemeNameUnique")]
        public IActionResult IsThemeNameUnique(string name)
        {
            var isThemeNameUnique = _themeRules.IsThemeNameUnique(name);
            return Ok(isThemeNameUnique);
        }

        [HttpGet("{folder}")]
        [Route("isThemeFolderUnique")]
        public IActionResult IsThemeFolderUnique(string folder)
        {
            var isThemeFolderUnique = _themeRules.IsThemeFolderUnique(folder);
            return Ok(isThemeFolderUnique);
        }

        [HttpGet("{folder}")]
        [Route("isThemeFolderValid")]
        public IActionResult IsThemeFolderValid(string folder)
        {
            var isThemeFolderValid = _themeRules.IsThemeFolderValid(folder);
            return Ok(isThemeFolderValid);
        }

        [HttpGet]
        [Route("admin-list")]
        public async Task<IActionResult> AdminList()
        {
            var models = await _dispatcher.GetResultAsync<GetAllForAdmin, IEnumerable<ThemeAdminModel>>(new GetAllForAdmin());
            return Ok(models);
        }

        [HttpGet]
        [Route("{id}/admin-edit")]
        public async Task<IActionResult> AdminEdit(Guid id)
        {
            var model = await _dispatcher.GetResultAsync<GetForAdmin, ThemeAdminModel>(new GetForAdmin { Id = id });

            if (model == null)
                return NotFound();

            return Ok(model);
        }
    }
}
