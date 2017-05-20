using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Weapsy.Domain.ModuleTypes;
using Weapsy.Framework.Queries;
using Weapsy.Reporting.Apps;
using Weapsy.Reporting.Apps.Queries;

namespace Weapsy.Data.Reporting.Apps
{
    public class GetModuleTypeAdminListModelHandler : IQueryHandlerAsync<GetModuleTypeAdminListModel, IEnumerable<ModuleTypeAdminListModel>>
    {
        private readonly IContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetModuleTypeAdminListModelHandler(IContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ModuleTypeAdminListModel>> RetrieveAsync(GetModuleTypeAdminListModel query)
        {
            using (var context = _contextFactory.Create())
            {
                var dbEntities = await context.ModuleTypes
                    .Where(x => x.Status != ModuleTypeStatus.Deleted)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<ModuleTypeAdminListModel>>(dbEntities);
            }
        }
    }
}
