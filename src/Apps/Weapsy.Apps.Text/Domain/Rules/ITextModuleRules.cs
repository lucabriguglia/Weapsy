using System;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Apps.Text.Domain.Rules
{
    public interface ITextModuleRules : IRules<TextModule>
    {
        bool IsTextModuleIdUnique(Guid id);
    }
}
