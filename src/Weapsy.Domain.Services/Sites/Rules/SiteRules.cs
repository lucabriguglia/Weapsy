using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Weapsy.Data;

namespace Weapsy.Domain.Services.Sites.Rules
{
    public class SiteRules : ISiteRules
    {
        private readonly WeapsyDbContext _dbContext;

        public SiteRules(WeapsyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> IsNameUniqueAsync(string name)
        {
            var siteNameExist = await _dbContext.Sites.AnyAsync(x => x.Name == name);
            return siteNameExist == false;
        }
    }
}
