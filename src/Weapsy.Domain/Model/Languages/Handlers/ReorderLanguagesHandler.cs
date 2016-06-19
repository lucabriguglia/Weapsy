using System.Collections.Generic;
using Weapsy.Core.Domain;
using Weapsy.Domain.Model.Languages.Commands;
using System;
using Weapsy.Domain.Model.Sites.Rules;

namespace Weapsy.Domain.Model.Languages.Handlers
{
    public class ReorderLanguagesHandler : ICommandHandler<ReorderLanguages>
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly ISiteRules _siteRules;

        public ReorderLanguagesHandler(ILanguageRepository languageRepository, ISiteRules siteRules)
        {
            _languageRepository = languageRepository;
            _siteRules = siteRules;
        }

        public ICollection<IEvent> Handle(ReorderLanguages cmd)
        {
            if (!_siteRules.DoesSiteExist(cmd.SiteId))
                throw new Exception("Site does not exist.");

            var events = new List<IEvent>();
            var updatedLanguages = new List<Language>();

            for (int i = 0; i < cmd.Languages.Count; i++)
            {
                var languageId = cmd.Languages[i];
                var sortOrder = i + 1;

                var language = _languageRepository.GetById(cmd.SiteId, languageId);

                if (language == null)
                    throw new Exception("Language not found.");

                if (language.SortOrder != sortOrder)
                {
                    language.Reorder(sortOrder);
                    updatedLanguages.Add(language);
                    events.AddRange(language.Events);
                }
            }

            _languageRepository.Update(updatedLanguages);

            return events;
        }
    }
}
