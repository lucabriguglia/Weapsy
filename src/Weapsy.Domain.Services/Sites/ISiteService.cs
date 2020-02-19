using System.Threading.Tasks;
using Weapsy.Core;
using Weapsy.Domain.Models;
using Weapsy.Domain.Models.Sites.Commands;

namespace Weapsy.Domain.Services.Sites
{
    public interface ISiteService
    {
        Task<CommandResponse> CreateAsync(CreateSite command);
        Task<CommandResponse> UpdateAsync(UpdateSite command);
    }
}
