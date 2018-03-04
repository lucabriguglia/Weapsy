using System.Threading.Tasks;
using FluentValidation;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;
using Weapsy.Domain.Languages.Commands;

namespace Weapsy.Domain.Languages.Handlers
{
    public class CreateLanguageHandler : ICommandHandlerWithAggregateAsync<CreateLanguage>
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

        public async Task<IAggregateRoot> HandleAsync(CreateLanguage command)
        {
            var language = Language.CreateNew(command, _validator, _sortOrderGenerator);

            await _languageRepository.CreateAsync(language);

            return language;
        }
    }
}
