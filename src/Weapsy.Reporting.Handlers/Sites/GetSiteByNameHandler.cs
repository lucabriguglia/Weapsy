using AutoMapper;
using Kledex.Queries;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Weapsy.Data;
using Weapsy.Reporting.Models.Sites;
using Weapsy.Reporting.Models.Sites.Queries;

namespace Weapsy.Reporting.Handlers
{
    public class GetSiteByNameHandler : IQueryHandlerAsync<GetSiteByName, SiteModel>
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly IMapper _mapper;

        public GetSiteByNameHandler(IDbContextFactory dbContextFactory, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
        }

        public async Task<SiteModel> HandleAsync(GetSiteByName query)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = await context.Sites.FirstOrDefaultAsync(x => x.Name == query.SiteName);
                return dbEntity != null ? _mapper.Map<SiteModel>(dbEntity) : null;
            }
        }
    }
}
