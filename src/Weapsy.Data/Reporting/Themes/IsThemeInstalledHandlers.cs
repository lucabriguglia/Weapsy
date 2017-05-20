using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Weapsy.Domain.Themes;
using Weapsy.Framework.Queries;
using Weapsy.Reporting.Themes.Queries;

namespace Weapsy.Data.Reporting.Themes
{
    public class IsThemeInstalledHandlers : IQueryHandler<IsThemeInstalled, bool>, IQueryHandlerAsync<IsThemeInstalled, bool>
    {
        private readonly IContextFactory _contextFactory;

        public IsThemeInstalledHandlers(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public bool Retrieve(IsThemeInstalled query)
        {
            using (var context = _contextFactory.Create())
            {
                return context.Themes.Where(x => x.Name == query.Name && x.Status == ThemeStatus.Active).Any();
            }
        }

        public async Task<bool> RetrieveAsync(IsThemeInstalled query)
        {
            using (var context = _contextFactory.Create())
            {
                return await context.Themes.Where(x => x.Name == query.Name && x.Status == ThemeStatus.Active).AnyAsync();
            }
        }
    }
}
