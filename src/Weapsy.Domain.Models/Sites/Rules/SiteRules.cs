using System.Threading.Tasks;

namespace Weapsy.Domain.Models.Sites.Rules
{
    public class SiteRules : ISiteRules
    {
        public Task<bool> IsNameUniqueAsync(string name)
        {
            // TO DO: Check the database

            return Task.FromResult(true);
        }
    }
}
