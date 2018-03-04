using System.Threading.Tasks;
using FluentValidation;
using Weapsy.Apps.Text.Domain.Commands;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Apps.Text.Domain.Handlers
{
    public class CreateTextModuleHandler : ICommandHandlerWithAggregateAsync<CreateTextModule>
    {
        private readonly ITextModuleRepository _textModuleRepository;
        private readonly IValidator<CreateTextModule> _validator;

        public CreateTextModuleHandler(ITextModuleRepository textModuleRepository,
            IValidator<CreateTextModule> validator)
        {
            _textModuleRepository = textModuleRepository;
            _validator = validator;
        }

        public async Task<IAggregateRoot> HandleAsync(CreateTextModule command)
        {
            var textModule = TextModule.CreateNew(command, _validator);

            await _textModuleRepository.CreateAsync(textModule);

            return textModule;
        }
    }
}
