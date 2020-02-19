using System.Threading.Tasks;
using Weapsy.Core;
using Weapsy.Domain.Models.Pages.Commands;

namespace Weapsy.Domain.Services.Pages
{
    public interface IPageService
    {
        Task<CommandResponse> CreateAsync(CreatePage command);
    }
}
