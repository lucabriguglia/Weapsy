using System;
using System.Threading.Tasks;
using Weapsy.Mvc.Controllers;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Reporting.Languages;
using Weapsy.Domain.Languages.Commands;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Languages.Rules;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Queries;
using Weapsy.Mvc.Context;
using Weapsy.Reporting.Languages.Queries;

namespace Weapsy.Api
{
    [Route("api/[controller]")]
    public class LanguageController : BaseAdminController
    {
        private readonly ICommandSender _commandSender;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ILanguageRules _languageRules;

        public LanguageController(ICommandSender commandSender,
            IQueryDispatcher queryDispatcher,
            ILanguageRules languageRules,
            IContextService contextService)
            : base(contextService)
        {
            _commandSender = commandSender;
            _queryDispatcher = queryDispatcher;
            _languageRules = languageRules;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var model = await _queryDispatcher.DispatchAsync<GetAllActive, IEnumerable<LanguageInfo>>(new GetAllActive {SiteId = SiteId});
            return Ok(model);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var languages = await _queryDispatcher.DispatchAsync<GetAllActive, IEnumerable<LanguageInfo>>(new GetAllActive { SiteId = SiteId });

            var language = languages.FirstOrDefault(x => x.Id == id);

            if (language == null)
                return NotFound();

            return Ok(language);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateLanguage model)
        {
            model.SiteId = SiteId;
            await _commandSender.SendAsync<CreateLanguage, Language>(model);
            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/update")]
        public IActionResult UpdateDetails([FromBody] UpdateLanguageDetails model)
        {
            model.SiteId = SiteId;
            _commandSender.Send<UpdateLanguageDetails, Language>(model);
            return new NoContentResult();
        }

        [HttpPut]
        [Route("reorder")]
        public IActionResult ReorderLanguages([FromBody] List<Guid> model)
        {
            _commandSender.Send<ReorderLanguages, Language>(new ReorderLanguages
            {
                SiteId = SiteId,
                Languages = model
            });
            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/activate")]
        public IActionResult Activate(Guid id)
        {
            _commandSender.Send<ActivateLanguage, Language>(new ActivateLanguage
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
            _commandSender.Send<HideLanguage, Language>(new HideLanguage
            {
                SiteId = SiteId,
                Id = id
            });
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _commandSender.Send<DeleteLanguage, Language>(new DeleteLanguage
            {
                SiteId = SiteId,
                Id = id
            });
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
        [Route("admin-list")]
        public async Task<IActionResult> AdminList()
        {
            var models = await _queryDispatcher.DispatchAsync<GetAllForAdmin, IEnumerable<LanguageAdminModel>>(new GetAllForAdmin { SiteId = SiteId });
            return Ok(models);
        }

        [HttpGet]
        [Route("{id}/admin-edit")]
        public async Task<IActionResult> AdminEdit(Guid id)
        {
            var model = await _queryDispatcher.DispatchAsync<GetForAdmin, LanguageAdminModel>(new GetForAdmin { SiteId = SiteId, Id = id});

            if (model == null)
                return NotFound();

            return Ok(model);
        }
    }
}
