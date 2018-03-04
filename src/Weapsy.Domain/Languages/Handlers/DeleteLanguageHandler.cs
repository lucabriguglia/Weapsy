using FluentValidation;
using System;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;
using Weapsy.Domain.Languages.Commands;

namespace Weapsy.Domain.Languages.Handlers
{
    public class DeleteLanguageHandler : ICommandHandlerWithAggregate<DeleteLanguage>
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly IValidator<DeleteLanguage> _validator;

        public DeleteLanguageHandler(ILanguageRepository languageRepository, IValidator<DeleteLanguage> validator)
        {
            _languageRepository = languageRepository;
            _validator = validator;
        }

        public IAggregateRoot Handle(DeleteLanguage command)
        {
            var language = _languageRepository.GetById(command.SiteId, command.Id);

            if (language == null)
                throw new Exception("Language not found.");

            language.Delete(command, _validator);

            _languageRepository.Update(language);

            return language;
        }
    }
}
