using System.Threading.Tasks;
using Weapsy.Core.Domain;
using Weapsy.Domain.Models.Sites.Commands;

namespace Weapsy.Domain.Services.Sites
{
    public interface ISiteService
    {
        Task<CommandResponse> CreateAsync(CreateSite command);
        Task<CommandResponse> UpdateAsync(UpdateSite command);
    }
}
