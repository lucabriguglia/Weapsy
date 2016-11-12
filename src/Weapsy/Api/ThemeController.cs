using System;
using System.Threading.Tasks;
using Weapsy.Mvc.Controllers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Reporting.Themes;
using Weapsy.Infrastructure.Dispatcher;
using Weapsy.Domain.Themes.Rules;
using Weapsy.Domain.Themes.Commands;
using Weapsy.Domain.Themes;
using Weapsy.Mvc.Context;

namespace Weapsy.Api
{
    [Route("api/[controller]")]
    public class ThemeController : BaseAdminController
    {
        private readonly IThemeFacade _themeFacade;
        private readonly ICommandSender _commandSender;
        private readonly IThemeRules _themeRules;

        public ThemeController(IThemeFacade themeFacade,
            ICommandSender commandSender,
            IThemeRules themeRules,
            IContextService contextService)
            : base(contextService)
        {
            _themeFacade = themeFacade;
            _commandSender = commandSender;
            _themeRules = themeRules;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var themes = await Task.Run(() => _themeFacade.GetAllForAdmin());
            return Ok(themes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var theme = await Task.Run(() => _themeFacade.GetForAdmin(id));

            if (theme == null)
                return NotFound();

            return Ok(theme);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateTheme model)
        {
            model.Id = Guid.NewGuid();
            await Task.Run(() => _commandSender.Send<CreateTheme, Theme>(model));
            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/update")]
        public async Task<IActionResult> UpdateDetails([FromBody] UpdateThemeDetails model)
        {
            await Task.Run(() => _commandSender.Send<UpdateThemeDetails, Theme>(model));
            return new NoContentResult();
        }

        [HttpPut]
        [Route("reorder")]
        public async Task<IActionResult> ReorderThemes([FromBody] List<Guid> model)
        {
            await Task.Run(() => _commandSender.Send<ReorderThemes, Theme>(new ReorderThemes { Themes = model }));
            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/activate")]
        public async Task<IActionResult> Activate(Guid id)
        {
            await Task.Run(() => _commandSender.Send<ActivateTheme, Theme>(new ActivateTheme { Id = id }));
            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/hide")]
        public async Task<IActionResult> Hide(Guid id)
        {
            await Task.Run(() => _commandSender.Send<HideTheme, Theme>(new HideTheme { Id = id }));
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await Task.Run(() => _commandSender.Send<DeleteTheme, Theme>(new DeleteTheme { Id = id }));
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
        [Route("{id}/admin-list")]
        public async Task<IActionResult> AdminList()
        {
            var model = await Task.Run(() => _themeFacade.GetAllForAdmin());
            return Ok(model);
        }

        [HttpGet]
        [Route("{id}/admin-edit")]
        public async Task<IActionResult> AdminEdit(Guid id)
        {
            var model = await Task.Run(() => _themeFacade.GetForAdmin(id));

            if (model == null)
                return NotFound();

            return Ok(model);
        }
    }
}
