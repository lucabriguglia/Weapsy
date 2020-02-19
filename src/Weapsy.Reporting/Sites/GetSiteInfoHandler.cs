using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Weapsy.Data;
using Weapsy.Reporting.Models.Sites;
using Weapsy.Reporting.Models.Sites.Queries;

namespace Weapsy.Reporting.Handlers
{
    public class GetSiteInfoHandler
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly IMapper _mapper;

        public GetSiteInfoHandler(IDbContextFactory dbContextFactory, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
        }

        public async Task<SiteInfo> HandleAsync(GetSiteInfo query)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = await context.Sites.FirstOrDefaultAsync(x => x.Name == query.SiteName);
                return dbEntity != null ? _mapper.Map<SiteInfo>(dbEntity) : null;
            }
        }
    }
}
