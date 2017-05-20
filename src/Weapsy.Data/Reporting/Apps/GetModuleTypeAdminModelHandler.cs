using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Weapsy.Domain.Apps;
using Weapsy.Domain.ModuleTypes;
using Weapsy.Framework.Queries;
using Weapsy.Reporting.Apps;
using Weapsy.Reporting.Apps.Queries;

namespace Weapsy.Data.Reporting.Apps
{
    public class GetModuleTypeAdminModelHandler : IQueryHandlerAsync<GetModuleTypeAdminModel, ModuleTypeAdminModel>
    {
        private readonly IContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetModuleTypeAdminModelHandler(IContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<ModuleTypeAdminModel> RetrieveAsync(GetModuleTypeAdminModel query)
        {
            using (var context = _contextFactory.Create())
            {
                var dbEntity = await context.ModuleTypes
                    .FirstOrDefaultAsync(x => x.Id == query.Id && x.Status != ModuleTypeStatus.Deleted);

                if (dbEntity == null)
                    return null;

                var result = _mapper.Map<ModuleTypeAdminModel>(dbEntity);

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
