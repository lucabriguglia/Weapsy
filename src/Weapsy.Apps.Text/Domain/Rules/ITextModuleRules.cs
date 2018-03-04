using System;

namespace Weapsy.Apps.Text.Domain.Rules
{
    public interface ITextModuleRules
    {
        bool IsTextModuleIdUnique(Guid id);
    }
}
