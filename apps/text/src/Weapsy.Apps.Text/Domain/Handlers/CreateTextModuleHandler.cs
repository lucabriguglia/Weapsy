using System.Collections.Generic;
using FluentValidation;
using Weapsy.Core.Domain;
using Weapsy.Apps.Text.Domain.Commands;

namespace Weapsy.Apps.Text.Domain.Handlers
{
    public class CreateTextModuleHandler : ICommandHandler<CreateTextModule>
    {
        private readonly ITextModuleRepository _textModuleRepository;
        private readonly IValidator<CreateTextModule> _validator;

        public CreateTextModuleHandler(ITextModuleRepository textModuleRepository,
            IValidator<CreateTextModule> validator)
        {
            _textModuleRepository = textModuleRepository;
            _validator = validator;
        }

        public ICollection<IEvent> Handle(CreateTextModule command)
        {
            var textModule = TextModule.CreateNew(command, _validator);

            _textModuleRepository.Create(textModule);

            return textModule.Events;
        }
    }
}
