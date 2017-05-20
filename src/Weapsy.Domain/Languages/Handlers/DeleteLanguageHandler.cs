using FluentValidation;
using System;
using System.Collections.Generic;
using Weapsy.Domain.Languages.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.Languages.Handlers
{
    public class DeleteLanguageHandler : ICommandHandler<DeleteLanguage>
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly IValidator<DeleteLanguage> _validator;

        public DeleteLanguageHandler(ILanguageRepository languageRepository, IValidator<DeleteLanguage> validator)
        {
            _languageRepository = languageRepository;
            _validator = validator;
        }

        public IEnumerable<IEvent> Handle(DeleteLanguage command)
        {
            var language = _languageRepository.GetById(command.SiteId, command.Id);

            if (language == null)
                throw new Exception("Language not found.");

            language.Delete(command, _validator);

            _languageRepository.Update(language);

            return language.Events;
        }
    }
}
