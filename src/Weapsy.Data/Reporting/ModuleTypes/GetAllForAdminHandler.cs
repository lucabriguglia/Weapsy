using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Weapsy.Domain.ModuleTypes;
using Weapsy.Infrastructure.Queries;
using Weapsy.Reporting.ModuleTypes;
using Weapsy.Reporting.ModuleTypes.Queries;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Weapsy.Data.Reporting.ModuleTypes
{
    public class GetAllForAdminHandler : IQueryHandlerAsync<GetAllForAdmin, IEnumerable<ModuleTypeAdminListModel>>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetAllForAdminHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ModuleTypeAdminListModel>> RetrieveAsync(GetAllForAdmin query)
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
