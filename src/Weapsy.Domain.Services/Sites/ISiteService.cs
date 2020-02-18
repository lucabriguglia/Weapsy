using System.Threading.Tasks;
using Weapsy.Domain.Models.Sites.Commands;

namespace Weapsy.Domain.Services.Sites
{
    public interface ISiteService
    {
        Task CreateAsync(CreateSite command);
        Task UpdateAsync(UpdateSite command);
    }
}
