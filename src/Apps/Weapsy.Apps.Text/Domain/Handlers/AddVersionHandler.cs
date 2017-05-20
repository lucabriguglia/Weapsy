using System.Collections.Generic;
using FluentValidation;
using Weapsy.Apps.Text.Domain.Commands;
using System;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Apps.Text.Domain.Handlers
{
    public class AddVersionHandler : ICommandHandler<AddVersion>
    {
        private readonly ITextModuleRepository _textModuleRepository;
        private readonly IValidator<AddVersion> _validator;

        public AddVersionHandler(ITextModuleRepository textModuleRepository,
            IValidator<AddVersion> validator)
        {
            _textModuleRepository = textModuleRepository;
            _validator = validator;
        }

        public IEnumerable<IEvent> Handle(AddVersion command)
        {
            var textModule = _textModuleRepository.GetByModuleId(command.ModuleId);

            if (textModule == null)
                throw new Exception("Text module not found.");

            textModule.AddVersion(command, _validator);

            _textModuleRepository.Update(textModule);

            return textModule.Events;
        }
    }
}
