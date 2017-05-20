using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Weapsy.Domain.Apps;
using Weapsy.Framework.Queries;
using Weapsy.Reporting.Apps;
using Weapsy.Reporting.Apps.Queries;

namespace Weapsy.Data.Reporting.Apps
{
    public class GetAppAdminModelHandler : IQueryHandlerAsync<GetAppAdminModel, AppAdminModel>
    {
        private readonly IContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetAppAdminModelHandler(IContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<AppAdminModel> RetrieveAsync(GetAppAdminModel query)
        {
            using (var context = _contextFactory.Create())
            {
                var dbEntity = await context.Apps.FirstOrDefaultAsync(x => x.Id == query.Id && x.Status != AppStatus.Deleted);
                return dbEntity != null ? _mapper.Map<AppAdminModel>(dbEntity) : null;
            }
        }
    }
}
