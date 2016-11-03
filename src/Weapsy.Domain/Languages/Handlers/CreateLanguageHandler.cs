using System.Collections.Generic;
using FluentValidation;
using Weapsy.Infrastructure.Domain;
using Weapsy.Domain.Languages.Commands;

namespace Weapsy.Domain.Languages.Handlers
{
    public class CreateLanguageHandler : ICommandHandler<CreateLanguage>
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly IValidator<CreateLanguage> _validator;
        private readonly ILanguageSortOrderGenerator _sortOrderGenerator;

        public CreateLanguageHandler(ILanguageRepository languageRepository,
            IValidator<CreateLanguage> validator,
            ILanguageSortOrderGenerator sortOrderGenerator)
        {
            _languageRepository = languageRepository;
            _validator = validator;
            _sortOrderGenerator = sortOrderGenerator;
        }

        public ICollection<IEvent> Handle(CreateLanguage command)
        {
            var language = Language.CreateNew(command, _validator, _sortOrderGenerator);

            _languageRepository.Create(language);

            return language.Events;
        }
    }
}
