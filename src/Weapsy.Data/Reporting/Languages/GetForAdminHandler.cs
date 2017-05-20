using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Weapsy.Domain.Languages;
using Weapsy.Framework.Queries;
using Weapsy.Reporting.Languages;
using Weapsy.Reporting.Languages.Queries;

namespace Weapsy.Data.Reporting.Languages
{
    public class GetForAdminHandler : IQueryHandlerAsync<GetForAdmin, LanguageAdminModel>
    {
        private readonly IContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetForAdminHandler(IContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<LanguageAdminModel> RetrieveAsync(GetForAdmin query)
        {
            using (var context = _contextFactory.Create())
            {
                var dbEntity = await context.Languages.FirstOrDefaultAsync(x => x.Id == query.Id && x.Status != LanguageStatus.Deleted);
                return dbEntity != null ? _mapper.Map<LanguageAdminModel>(dbEntity) : null;
            }
        }
    }
}
