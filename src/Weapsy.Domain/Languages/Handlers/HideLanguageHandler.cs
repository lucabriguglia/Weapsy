using FluentValidation;
using System;
using System.Collections.Generic;
using Weapsy.Domain.Languages.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.Languages.Handlers
{
    public class HideLanguageHandler : ICommandHandler<HideLanguage>
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly IValidator<HideLanguage> _validator;

        public HideLanguageHandler(ILanguageRepository languageRepository, IValidator<HideLanguage> validator)
        {
            _languageRepository = languageRepository;
            _validator = validator;
        }

        public IEnumerable<IEvent> Handle(HideLanguage command)
        {
            var language = _languageRepository.GetById(command.SiteId, command.Id);

            if (language == null)
                throw new Exception("Language not found.");

            language.Hide(command, _validator);

            _languageRepository.Update(language);

            return language.Events;
        }
    }
}
