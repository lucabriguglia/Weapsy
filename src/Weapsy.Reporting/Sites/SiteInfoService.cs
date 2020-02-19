using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Weapsy.Data;
using Weapsy.Reporting.Sites.Models;
using Weapsy.Reporting.Sites.Queries;

namespace Weapsy.Reporting.Sites
{
    public class SiteInfoService : ISiteInfoService
    {
        private readonly WeapsyDbContext _dbContext;
        private readonly IMapper _mapper;

        public SiteInfoService(WeapsyDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<SiteInfo> GetSiteInfoAsync(GetSiteInfo query)
        {
            var dbEntity = await _dbContext.Sites.FirstOrDefaultAsync(x => x.Name == query.SiteName);
            return dbEntity != null ? _mapper.Map<SiteInfo>(dbEntity) : null;
        }
    }
}
