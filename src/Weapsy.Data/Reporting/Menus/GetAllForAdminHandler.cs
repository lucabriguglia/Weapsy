using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Weapsy.Domain.Menus;
using Weapsy.Framework.Queries;
using Weapsy.Reporting.Menus;
using Weapsy.Reporting.Menus.Queries;

namespace Weapsy.Data.Reporting.Menus
{
    public class GetAllForAdminHandler : IQueryHandlerAsync<GetAllForAdmin, IEnumerable<MenuAdminModel>>
    {
        private readonly IContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetAllForAdminHandler(IContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MenuAdminModel>> RetrieveAsync(GetAllForAdmin query)
        {
            using (var context = _contextFactory.Create())
            {
                var entities = await context.Menus.Include(x => x.MenuItems)
                    .Where(x => x.SiteId == query.SiteId && x.Status != MenuStatus.Deleted)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<MenuAdminModel>>(entities);
            }
        }
    }
}
