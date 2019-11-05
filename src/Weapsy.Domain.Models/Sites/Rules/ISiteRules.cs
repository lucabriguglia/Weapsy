using System.Threading.Tasks;

namespace Weapsy.Domain.Models.Sites.Rules
{
    public interface ISiteRules
    {
        Task<bool> IsNameUniqueAsync(string name);
    }
}
