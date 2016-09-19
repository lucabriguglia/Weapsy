using FluentValidation;
using System;
using System.Collections.Generic;
using Weapsy.Core.Domain;
using Weapsy.Domain.Languages.Commands;

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

        public ICollection<IEvent> Handle(DeleteLanguage command)
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
