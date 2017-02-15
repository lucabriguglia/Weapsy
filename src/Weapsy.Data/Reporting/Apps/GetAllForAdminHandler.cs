using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Weapsy.Domain.Apps;
using Weapsy.Infrastructure.Queries;
using Weapsy.Reporting.Apps;
using Weapsy.Reporting.Apps.Queries;

namespace Weapsy.Data.Reporting.Apps
{
    public class GetAllForAdminHandler : IQueryHandlerAsync<GetAllForAdmin, IEnumerable<AppAdminListModel>>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetAllForAdminHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AppAdminListModel>> RetrieveAsync(GetAllForAdmin query)
        {
            using (var context = _contextFactory.Create())
            {
                var entities = await context.Apps
                    .Where(x => x.Status != AppStatus.Deleted)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<AppAdminListModel>>(entities);
            }
        }
    }
}
