using System.Threading.Tasks;
using Weapsy.Infrastructure.Queries;
using Weapsy.Reporting.ModuleTypes;
using Weapsy.Reporting.ModuleTypes.Queries;
using System.Linq;
using Weapsy.Domain.Apps;
using Microsoft.EntityFrameworkCore;

namespace Weapsy.Data.Reporting.ModuleTypes
{
    public class GetDefaultForAdminHandler : IQueryHandlerAsync<GetDefaultForAdmin, ModuleTypeAdminModel>
    {
        private readonly IDbContextFactory _contextFactory;

        public GetDefaultForAdminHandler(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<ModuleTypeAdminModel> RetrieveAsync(GetDefaultForAdmin query)
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
