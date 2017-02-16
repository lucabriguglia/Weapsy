using System.Threading.Tasks;
using AutoMapper;
using Weapsy.Domain.EmailAccounts;
using Weapsy.Infrastructure.Queries;
using Weapsy.Reporting.EmailAccounts;
using Weapsy.Reporting.EmailAccounts.Queries;
using Microsoft.EntityFrameworkCore;

namespace Weapsy.Data.Reporting.EmailAccounts
{
    public class GetEmailAccountHandler : IQueryHandlerAsync<GetEmailAccount, EmailAccountModel>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetEmailAccountHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<EmailAccountModel> RetrieveAsync(GetEmailAccount query)
        {
            using (var context = _contextFactory.Create())
            {
                var dbEntity = await context.EmailAccounts.FirstOrDefaultAsync(x => x.SiteId == query.SiteId && x.Id == query.Id && x.Status != EmailAccountStatus.Deleted);
                return dbEntity != null ? _mapper.Map<EmailAccountModel>(dbEntity) : null;
            }
        }
    }
}
