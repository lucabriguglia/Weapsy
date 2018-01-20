using System;

namespace Weapsy.Apps.Text.Domain.Rules
{
    public class TextModuleRules : ITextModuleRules
    {
        private readonly ITextModuleRepository _textModuleRepository;

        public TextModuleRules(ITextModuleRepository textModuleRepository)
        {
            _textModuleRepository = textModuleRepository;
        }

        public bool IsTextModuleIdUnique(Guid id)
        {
            return _textModuleRepository.GetById(id) == null;
        }
    }
}
