using System.Threading.Tasks;
using AutoMapper;
using Weapsy.Infrastructure.Queries;
using Weapsy.Reporting.ModuleTypes;
using Weapsy.Reporting.ModuleTypes.Queries;
using System.Linq;
using Weapsy.Data.Identity;
using Weapsy.Domain.Apps;

namespace Weapsy.Data.Reporting.ModuleTypes
{
    public class GetDefaultForAdminHandler : IQueryHandlerAsync<GetDefaultForAdmin, ModuleTypeAdminModel>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;

        public GetDefaultForAdminHandler(IDbContextFactory contextFactory, IMapper mapper, IRoleService roleService)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _roleService = roleService;
        }

        public async Task<ModuleTypeAdminModel> RetrieveAsync(GetDefaultForAdmin query)
        {
            using (var context = _contextFactory.Create())
            {
                var result = new ModuleTypeAdminModel();

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
