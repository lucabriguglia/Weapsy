using System;
using System.Threading.Tasks;

namespace Weapsy.Domain.Models.Sites
{
    public interface ISiteRepository
    {
        Task<Site> GetByIdAsync(Guid id);
        Task<bool> AnyByNameAsync(string name);
        Task CreateAsync(Site site);
        Task UpdateAsync(Site site);
    }
}
