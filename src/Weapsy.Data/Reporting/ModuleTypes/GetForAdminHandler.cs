using System.Threading.Tasks;
using AutoMapper;
using Weapsy.Domain.ModuleTypes;
using Weapsy.Infrastructure.Queries;
using Weapsy.Reporting.ModuleTypes;
using Weapsy.Reporting.ModuleTypes.Queries;
using System.Linq;
using Weapsy.Domain.Apps;

namespace Weapsy.Data.Reporting.ModuleTypes
{
    public class GetForAdminHandler : IQueryHandlerAsync<GetForAdmin, ModuleTypeAdminModel>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetForAdminHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<ModuleTypeAdminModel> RetrieveAsync(GetForAdmin query)
        {
            using (var context = _contextFactory.Create())
            {
                var dbEntity = context.ModuleTypes
                    .FirstOrDefault(x => x.Id == query.Id && x.Status != ModuleTypeStatus.Deleted);

                if (dbEntity == null)
                    return null;

                var result = _mapper.Map<ModuleTypeAdminModel>(dbEntity);

                var apps = context.Apps
                    .Where(x => x.Status != AppStatus.Deleted)
                    .Select(app => new ModuleTypeAdminModel.App
                    {
                        Id = app.Id,
                        Name = app.Name
                    }).ToList();

                result.AvailableApps.AddRange(apps);

                return result;
            }
        }
    }
}
