using FluentValidation;
using System;
using System.Collections.Generic;
using Weapsy.Domain.Languages.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.Languages.Handlers
{
    public class DeleteLanguageHandler : ICommandHandler<DeleteLanguageCommand>
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly IValidator<DeleteLanguageCommand> _validator;

        public DeleteLanguageHandler(ILanguageRepository languageRepository, IValidator<DeleteLanguageCommand> validator)
        {
            _languageRepository = languageRepository;
            _validator = validator;
        }

        public IEnumerable<IEvent> Handle(DeleteLanguageCommand command)
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
