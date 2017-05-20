using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Weapsy.Domain.Apps;
using Weapsy.Framework.Queries;
using Weapsy.Reporting.Apps.Queries;

namespace Weapsy.Data.Reporting.Apps
{
    public class IsAppInstalledHandlers : IQueryHandler<IsAppInstalled, bool>, IQueryHandlerAsync<IsAppInstalled, bool>
    {
        private readonly IContextFactory _contextFactory;

        public IsAppInstalledHandlers(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public bool Retrieve(IsAppInstalled query)
        {
            using (var context = _contextFactory.Create())
            {
                return context.Apps.Where(x => x.Name == query.Name && x.Status == AppStatus.Active).Any();
            }
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
