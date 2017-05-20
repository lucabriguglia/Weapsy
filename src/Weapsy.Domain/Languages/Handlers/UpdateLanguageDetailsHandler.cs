using System.Collections.Generic;
using FluentValidation;
using Weapsy.Domain.Languages.Commands;
using System;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.Languages.Handlers
{
    public class UpdateLanguageDetailsHandler : ICommandHandler<UpdateLanguageDetails>
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly IValidator<UpdateLanguageDetails> _validator;

        public UpdateLanguageDetailsHandler(ILanguageRepository languageRepository, 
            IValidator<UpdateLanguageDetails> validator)
        {
            _languageRepository = languageRepository;
            _validator = validator;
        }

        public IEnumerable<IEvent> Handle(UpdateLanguageDetails command)
        {
            var language = _languageRepository.GetById(command.SiteId, command.Id);

            if (language == null)
                throw new Exception("Language not found.");

            language.UpdateDetails(command, _validator);

            _languageRepository.Update(language);

            return language.Events;
        }
    }
}
