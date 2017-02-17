using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Weapsy.Domain.Apps;
using Weapsy.Infrastructure.Queries;
using Weapsy.Reporting.Apps;
using Weapsy.Reporting.Apps.Queries;

namespace Weapsy.Data.Reporting.Apps
{
    public class GetDefaultModuleTypeAdminModelHandler : IQueryHandlerAsync<GetDefaultModuleTypeAdminModel, ModuleTypeAdminModel>
    {
        private readonly IDbContextFactory _contextFactory;

        public GetDefaultModuleTypeAdminModelHandler(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<ModuleTypeAdminModel> RetrieveAsync(GetDefaultModuleTypeAdminModel query)
        {
            using (var context = _contextFactory.Create())
            {
                var result = new ModuleTypeAdminModel();

                var apps = await context.Apps
                    .Where(x => x.Status != AppStatus.Deleted)
                    .Select(app => new ModuleTypeAdminModel.App
                    {
                        Id = app.Id,
                        Name = app.Name
                    }).ToListAsync();

                result.AvailableApps.AddRange(apps);

                return result;
            }
        }
    }
}
