using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Weapsy.Domain.Apps;
using Weapsy.Infrastructure.Queries;
using Weapsy.Reporting.Apps.Queries;

namespace Weapsy.Data.Reporting.Apps
{
    public class IsAppInstalledHandler : IQueryHandlerAsync<IsAppInstalled, bool>
    {
        private readonly IContextFactory _contextFactory;

        public IsAppInstalledHandler(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<bool> RetrieveAsync(IsAppInstalled query)
        {
            using (var context = _contextFactory.Create())
            {
                return await context.Apps.Where(x => x.Name == query.Name && x.Status == AppStatus.Active).AnyAsync();
            }
        }
    }
}
