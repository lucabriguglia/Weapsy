using Weapsy.Domain.Languages.Commands;
using System;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Languages.Handlers
{
    public class ReorderLanguageHandler : ICommandHandlerWithAggregate<ReorderLanguage>
    {
        private readonly ILanguageRepository _languageRepository;

        public ReorderLanguageHandler(ILanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;
        }

        public IAggregateRoot Handle(ReorderLanguage cmd)
        {
            var language = _languageRepository.GetById(cmd.SiteId, cmd.AggregateRootId);

            if (language == null)
                throw new Exception("Language not found.");

            language.Reorder(cmd.Order);
            _languageRepository.Update(language);

            return language;
        }
    }
}
