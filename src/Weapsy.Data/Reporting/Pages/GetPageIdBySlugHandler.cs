using System;
using System.Threading.Tasks;
using Weapsy.Domain.Pages;
using Weapsy.Infrastructure.Queries;
using Weapsy.Reporting.Pages.Queries;
using Microsoft.EntityFrameworkCore;

namespace Weapsy.Data.Reporting.Pages
{
    public class GetPageIdBySlugHandler : IQueryHandlerAsync<GetPageIdBySlug, Guid?>
    {
        private readonly IDbContextFactory _contextFactory;

        public GetPageIdBySlugHandler(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Guid?> RetrieveAsync(GetPageIdBySlug query)
        {
            using (var context = _contextFactory.Create())
            {
                if (query.LanguageId == Guid.Empty)
                {
                    var dbEntity = await context.Pages
                        .FirstOrDefaultAsync(x => x.SiteId == query.SiteId
                        && x.Status == PageStatus.Active
                        && x.Url == query.Slug);

                    return dbEntity?.Id;
                }
                else
                {
                    var dbEntity = await context.PageLocalisations
                        .FirstOrDefaultAsync(x => x.Page.SiteId == query.SiteId
                        && x.Page.Status == PageStatus.Active
                        && x.Url == query.Slug
                        && x.LanguageId == query.LanguageId);

                    return dbEntity?.PageId;
                }
            }
        }
    }
}
