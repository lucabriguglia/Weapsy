using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Weapsy.Domain.EmailAccounts;
using Weapsy.Reporting.EmailAccounts;
using Weapsy.Reporting.EmailAccounts.Queries;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Weapsy.Cqrs.Queries;

namespace Weapsy.Data.Reporting.EmailAccounts
{
    public class GetAllEmailAccountsHandler : IQueryHandlerAsync<GetAllEmailAccounts, IEnumerable<EmailAccountModel>>
    {
        private readonly IContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetAllEmailAccountsHandler(IContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmailAccountModel>> RetrieveAsync(GetAllEmailAccounts query)
        {
            using (var context = _contextFactory.Create())
            {
                var dbEntities = await context.EmailAccounts
                    .Where(x => x.SiteId == query.SiteId && x.Status != EmailAccountStatus.Deleted)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<EmailAccountModel>>(dbEntities);
            }
        }
    }
}
