using System;
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
        private readonly ICommandSender _commandSender;
        private readonly IThemeFacade _themeFacade;        
        private readonly IThemeRules _themeRules;

        public ThemeController(ICommandSender commandSender, 
            IThemeFacade themeFacade,           
            IThemeRules themeRules,
            IContextService contextService)
            : base(contextService)
        {
            _commandSender = commandSender;
            _themeFacade = themeFacade;
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
        [Route("{id}/admin-list")]
        public IActionResult AdminList()
        {
            var model = _themeFacade.GetAllForAdmin();
            return Ok(model);
        }

        [HttpGet]
        [Route("{id}/admin-edit")]
        public IActionResult AdminEdit(Guid id)
        {
            var model = _themeFacade.GetForAdmin(id);
            if (model == null)
                return NotFound();
            return Ok(model);
        }
    }
}
