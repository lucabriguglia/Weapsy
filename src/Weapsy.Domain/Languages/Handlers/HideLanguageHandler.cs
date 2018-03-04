using FluentValidation;
using System;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;
using Weapsy.Domain.Languages.Commands;

namespace Weapsy.Domain.Languages.Handlers
{
    public class HideLanguageHandler : ICommandHandlerWithAggregate<HideLanguage>
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly IValidator<HideLanguage> _validator;

        public HideLanguageHandler(ILanguageRepository languageRepository, IValidator<HideLanguage> validator)
        {
            _languageRepository = languageRepository;
            _validator = validator;
        }

        public IAggregateRoot Handle(HideLanguage command)
        {
            var language = _languageRepository.GetById(command.SiteId, command.Id);

            if (language == null)
                throw new Exception("Language not found.");

            language.Hide(command, _validator);

            _languageRepository.Update(language);

            return language;
        }
    }
}
