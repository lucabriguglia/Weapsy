using System;
using Weapsy.Mvc.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Domain.Themes.Rules;
using Weapsy.Domain.Themes.Commands;
using Weapsy.Domain.Themes;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Queries;
using Weapsy.Mvc.Context;
using Weapsy.Reporting.Themes;
using Weapsy.Reporting.Themes.Queries;

namespace Weapsy.Api
{
    [Route("api/[controller]")]
    public class ThemeController : BaseAdminController
    {
        private readonly ICommandSender _commandSender;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly IThemeRules _themeRules;

        public ThemeController(ICommandSender commandSender,
            IQueryDispatcher queryDispatcher,           
            IThemeRules themeRules,
            IContextService contextService)
            : base(contextService)
        {
            _commandSender = commandSender;
            _queryDispatcher = queryDispatcher;
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
            _commandSender.Send<CreateTheme, Theme>(model);
            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/update")]
        public IActionResult UpdateDetails([FromBody] UpdateThemeDetails model)
        {
            _commandSender.Send<UpdateThemeDetails, Theme>(model);
            return new NoContentResult();
        }

        [HttpPut]
        [Route("reorder")]
        public IActionResult ReorderThemes([FromBody] List<Guid> model)
        {
            _commandSender.Send<ReorderThemes, Theme>(new ReorderThemes { Themes = model });
            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/activate")]
        public IActionResult Activate(Guid id)
        {
            _commandSender.Send<ActivateTheme, Theme>(new ActivateTheme { Id = id });
            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/hide")]
        public IActionResult Hide(Guid id)
        {
            _commandSender.Send<HideTheme, Theme>(new HideTheme { Id = id });
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _commandSender.Send<DeleteTheme, Theme>(new DeleteTheme { Id = id });
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
            var models = await _queryDispatcher.DispatchAsync<GetAllForAdmin, IEnumerable<ThemeAdminModel>>(new GetAllForAdmin());
            return Ok(models);
        }

        [HttpGet]
        [Route("{id}/admin-edit")]
        public async Task<IActionResult> AdminEdit(Guid id)
        {
            var model = await _queryDispatcher.DispatchAsync<GetForAdmin, ThemeAdminModel>(new GetForAdmin { Id = id });

            if (model == null)
                return NotFound();

            return Ok(model);
        }
    }
}
