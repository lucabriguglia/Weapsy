using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Weapsy.Domain.Apps;
using Weapsy.Framework.Queries;
using Weapsy.Reporting.Apps;
using Weapsy.Reporting.Apps.Queries;

namespace Weapsy.Data.Reporting.Apps
{
    public class GetAppAdminModelListHandler : IQueryHandlerAsync<GetAppAdminModelList, IEnumerable<AppAdminListModel>>
    {
        private readonly IContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetAppAdminModelListHandler(IContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AppAdminListModel>> RetrieveAsync(GetAppAdminModelList query)
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
