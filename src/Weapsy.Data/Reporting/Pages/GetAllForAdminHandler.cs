using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Weapsy.Domain.Pages;
using Weapsy.Reporting.Pages;
using Weapsy.Reporting.Pages.Queries;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Weapsy.Cqrs.Queries;

namespace Weapsy.Data.Reporting.Pages
{
    public class GetAllForAdminHandler : IQueryHandlerAsync<GetAllForAdmin, IEnumerable<PageAdminListModel>>
    {
        private readonly IContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetAllForAdminHandler(IContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PageAdminListModel>> RetrieveAsync(GetAllForAdmin query)
        {
            using (var context = _contextFactory.Create())
            {
                var pages = await context.Pages
                    .Include(x => x.PageLocalisations)
                    .Where(x => x.SiteId == query.SiteId && x.Status != PageStatus.Deleted)
                    .OrderBy(x => x.Name).ToListAsync();

                return _mapper.Map<IEnumerable<PageAdminListModel>>(pages);
            }
        }
    }
}
