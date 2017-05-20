using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using Weapsy.Domain.Languages.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.Languages.Handlers
{
    public class CreateLanguageHandler : ICommandHandlerAsync<CreateLanguage>
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

        public async Task<IEnumerable<IEvent>> HandleAsync(CreateLanguage command)
        {
            var language = Language.CreateNew(command, _validator, _sortOrderGenerator);

            await _languageRepository.CreateAsync(language);

            return language.Events;
        }
    }
}
