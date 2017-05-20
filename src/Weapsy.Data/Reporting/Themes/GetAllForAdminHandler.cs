using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Weapsy.Domain.Themes;
using Weapsy.Reporting.Themes;
using Weapsy.Reporting.Themes.Queries;
using Microsoft.EntityFrameworkCore;
using Weapsy.Framework.Queries;

namespace Weapsy.Data.Reporting.Themes
{
    public class GetAllForAdminHandler : IQueryHandlerAsync<GetAllForAdmin, IEnumerable<ThemeAdminModel>>
    {
        private readonly IContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetAllForAdminHandler(IContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ThemeAdminModel>> RetrieveAsync(GetAllForAdmin query)
        {
            using (var context = _contextFactory.Create())
            {
                var dbEntities = await context.Themes
                    .Where(x => x.Status != ThemeStatus.Deleted)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<ThemeAdminModel>>(dbEntities);
            }
        }
    }
}
