using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using Weapsy.Domain.Languages.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.Languages.Handlers
{
    public class CreateLanguageHandler : ICommandHandlerAsync<CreateLanguageCommand>
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly IValidator<CreateLanguageCommand> _validator;
        private readonly ILanguageSortOrderGenerator _sortOrderGenerator;

        public CreateLanguageHandler(ILanguageRepository languageRepository,
            IValidator<CreateLanguageCommand> validator,
            ILanguageSortOrderGenerator sortOrderGenerator)
        {
            _languageRepository = languageRepository;
            _validator = validator;
            _sortOrderGenerator = sortOrderGenerator;
        }

        public async Task<IEnumerable<IEvent>> HandleAsync(CreateLanguageCommand command)
        {
            var language = Language.CreateNew(command, _validator, _sortOrderGenerator);

            await _languageRepository.CreateAsync(language);

            return language.Events;
        }
    }
}
