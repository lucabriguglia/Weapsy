using System.Collections.Generic;
using Weapsy.Framework.Domain;
using Weapsy.Domain.Languages.Commands;
using System;
using Weapsy.Domain.Sites.Rules;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.Languages.Handlers
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

        public IEnumerable<IEvent> Handle(ReorderLanguages cmd)
        {
            if (!_siteRules.DoesSiteExist(cmd.SiteId))
                throw new Exception("Site does not exist.");

            var events = new List<IDomainEvent>();
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
