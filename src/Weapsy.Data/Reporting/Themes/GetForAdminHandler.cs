using System.Threading.Tasks;
using AutoMapper;
using Weapsy.Domain.Themes;
using Weapsy.Infrastructure.Queries;
using Weapsy.Reporting.Themes;
using Weapsy.Reporting.Themes.Queries;
using Microsoft.EntityFrameworkCore;

namespace Weapsy.Data.Reporting.Themes
{
    public class GetForAdminHandler : IQueryHandlerAsync<GetForAdmin, ThemeAdminModel>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetForAdminHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<ThemeAdminModel> RetrieveAsync(GetForAdmin query)
        {
            using (var context = _contextFactory.Create())
            {
                var dbEntity = await context.Themes.FirstOrDefaultAsync(x => x.Id == query.Id && x.Status != ThemeStatus.Deleted);
                return dbEntity != null ? _mapper.Map<ThemeAdminModel>(dbEntity) : null;
            }
        }
    }
}
