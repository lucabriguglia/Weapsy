using System;
using System.Threading.Tasks;

namespace Weapsy.Domain.Services.Sites.Rules
{
    public interface ISiteRules
    {
        Task<bool> IsNameUniqueAsync(string name, Guid id);
    }
}
