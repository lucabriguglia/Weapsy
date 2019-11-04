using System.Threading.Tasks;
using Weapsy.Domain.Apps.Commands;
using Weapsy.Reporting.Apps;
using Weapsy.Reporting.Apps.Queries;

namespace Weapsy.Application
{
    public interface IAppService
    {
        Task CreateAppAsync(CreateApp command);
        Task UpdateAppDetailsAsync(UpdateAppDetails command);
        Task<bool> IsAppInstalledAsync(string name);
        bool IsAppNameUnique(string name);
        Task<AppAdminModel> GetAppAdminModelAsync(GetAppAdminModel query);
    }
}