using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Weapsy.Domain.Sites;
using Weapsy.Framework.Queries;
using Weapsy.Reporting.Sites.Queries;

namespace Weapsy.Data.Reporting.Sites
{
    public class IsSiteInstalledHandler : IQueryHandlerAsync<IsSiteInstalled, bool>
    {
        private readonly IContextFactory _contextFactory;

        public IsSiteInstalledHandler(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<bool> RetrieveAsync(IsSiteInstalled query)
        {
            using (var context = _contextFactory.Create())
            {
                return await context.Sites.Where(x => x.Name == query.Name && x.Status == SiteStatus.Active).AnyAsync();
            }
        }
    }
}
