using System;
using Weapsy.Core.Domain;

namespace Weapsy.Apps.Text.Domain.Rules
{
    public interface ITextModuleRules : IRules<TextModule>
    {
        bool IsTextModuleIdUnique(Guid id);
    }
}
