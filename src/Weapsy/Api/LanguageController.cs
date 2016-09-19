using System;
using System.Threading.Tasks;
using Weapsy.Mvc.Controllers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Reporting.Languages;
using Weapsy.Domain.Languages.Commands;
using Weapsy.Domain.Languages;
using Weapsy.Core.Dispatcher;
using Weapsy.Domain.Languages.Rules;
using Weapsy.Mvc.Context;

namespace Weapsy.Api
{
    [Route("api/[controller]")]
    public class LanguageController : BaseAdminController
    {
        private readonly ILanguageFacade _languageFacade;
        private readonly ICommandSender _commandSender;
        private readonly ILanguageRules _languageRules;

        public LanguageController(ILanguageFacade languageFacade,
            ICommandSender commandSender,
            ILanguageRules languageRules,
            IContextService contextService)
            : base(contextService)
        {
            _languageFacade = languageFacade;
            _commandSender = commandSender;
            _languageRules = languageRules;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var languages = await _languageFacade.GetAllForAdminAsync(SiteId);
            return Ok(languages);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var language = await _languageFacade.GetForAdminAsync(SiteId, id);
            if (language == null)
                return NotFound();
            return Ok(language);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateLanguage model)
        {
            model.SiteId = SiteId;
            model.Id = Guid.NewGuid();
            await Task.Run(() => _commandSender.Send<CreateLanguage, Language>(model));
            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/update")]
        public async Task<IActionResult> UpdateDetails([FromBody] UpdateLanguageDetails model)
        {
            model.SiteId = SiteId;
            await Task.Run(() => _commandSender.Send<UpdateLanguageDetails, Language>(model));
            return new NoContentResult();
        }

        [HttpPut]
        [Route("reorder")]
        public async Task<IActionResult> ReorderLanguages([FromBody] List<Guid> model)
        {
            await Task.Run(() => _commandSender.Send<ReorderLanguages, Language>(new ReorderLanguages
            {
                SiteId = SiteId,
                Languages = model
            }));
            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/activate")]
        public async Task<IActionResult> Activate(Guid id)
        {
            await Task.Run(() => _commandSender.Send<ActivateLanguage, Language>(new ActivateLanguage
            {
                SiteId = SiteId,
                Id = id
            }));
            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/hide")]
        public async Task<IActionResult> Hide(Guid id)
        {
            await Task.Run(() => _commandSender.Send<HideLanguage, Language>(new HideLanguage
            {
                SiteId = SiteId,
                Id = id
            }));
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await Task.Run(() => _commandSender.Send<DeleteLanguage, Language>(new DeleteLanguage
            {
                SiteId = SiteId,
                Id = id
            }));
            return new NoContentResult();
        }

        [HttpGet("{name}")]
        [Route("isLanguageNameUnique")]
        public IActionResult IsLanguageNameUnique(string name)
        {
            var isLanguageNameUnique = _languageRules.IsLanguageNameUnique(SiteId, name);
            return Ok(isLanguageNameUnique);
        }

        [HttpGet("{name}")]
        [Route("isLanguageNameValid")]
        public IActionResult IsLanguageNameValid(string name)
        {
            var isLanguageNameValid = _languageRules.IsLanguageNameValid(name);
            return Ok(isLanguageNameValid);
        }

        [HttpGet("{cultureName}")]
        [Route("isCultureNameUnique")]
        public IActionResult IsCultureNameUnique(string cultureName)
        {
            var isCultureNameUnique = _languageRules.IsCultureNameUnique(SiteId, cultureName);
            return Ok(isCultureNameUnique);
        }

        [HttpGet("{cultureName}")]
        [Route("isCultureNameValid")]
        public IActionResult IsCultureNameValid(string cultureName)
        {
            var isCultureNameValid = _languageRules.IsCultureNameValid(cultureName);
            return Ok(isCultureNameValid);
        }

        [HttpGet("{url}")]
        [Route("isLanguageUrlUnique")]
        public IActionResult IsLanguageUrlUnique(string url)
        {
            var isLanguageUrlUnique = _languageRules.IsLanguageUrlUnique(SiteId, url);
            return Ok(isLanguageUrlUnique);
        }

        [HttpGet("{url}")]
        [Route("isLanguageUrlValid")]
        public IActionResult IsLanguageUrlValid(string url)
        {
            var isLanguageUrlValid = _languageRules.IsLanguageUrlValid(url);
            return Ok(isLanguageUrlValid);
        }

        [HttpGet]
        [Route("{id}/admin-list")]
        public async Task<IActionResult> AdminList()
        {
            var model = await _languageFacade.GetAllForAdminAsync(SiteId);
            return Ok(model);
        }

        [HttpGet]
        [Route("{id}/admin-edit")]
        public async Task<IActionResult> AdminEdit(Guid id)
        {
            var model = await _languageFacade.GetForAdminAsync(SiteId, id);
            if (model == null) return NotFound();
            return Ok(model);
        }
    }
}
