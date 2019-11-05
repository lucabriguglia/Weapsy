using System.Threading.Tasks;

namespace Weapsy.Domain.Models.Sites.Rules
{
    public class SiteRules : ISiteRules
    {
        private readonly ISiteRepository _repository;

        public SiteRules(ISiteRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> IsNameUniqueAsync(string name)
        {
            return await _repository.AnyByNameAsync(name) == false;
        }
    }
}
