using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Weapsy.Domain.Languages;
using Weapsy.Infrastructure.Queries;
using Weapsy.Reporting.Languages;
using Weapsy.Reporting.Languages.Queries;

namespace Weapsy.Data.Reporting.Languages
{
    public class GetAllForAdminHandler : IQueryHandlerAsync<GetAllForAdmin, IEnumerable<LanguageAdminModel>>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetAllForAdminHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LanguageAdminModel>> RetrieveAsync(GetAllForAdmin query)
        {
            using (var context = _contextFactory.Create())
            {
                var entities = await context.Languages
                    .Where(x => x.Status != LanguageStatus.Deleted)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<LanguageAdminModel>>(entities);
            }
        }
    }
}
