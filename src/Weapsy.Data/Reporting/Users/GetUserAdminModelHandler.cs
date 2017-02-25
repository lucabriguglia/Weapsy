using AutoMapper;
using Weapsy.Infrastructure.Queries;
using System.Threading.Tasks;
using Weapsy.Reporting.Users.Queries;
using Weapsy.Reporting.Users;
using Microsoft.EntityFrameworkCore;
using Weapsy.Domain.Users;

namespace Weapsy.Data.Reporting.Users
{
    public class GetUserAdminModelHandler : IQueryHandlerAsync<GetUserAdminModel, UserAdminModel>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetUserAdminModelHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<UserAdminModel> RetrieveAsync(GetUserAdminModel query)
        {
            using (var context = _contextFactory.Create())
            {
                var dbEntity = await context.Users.FirstOrDefaultAsync(x => x.Id == query.Id && x.Status != UserStatus.Deleted);
                return dbEntity != null ? _mapper.Map<UserAdminModel>(dbEntity) : null;
            }
        }
    }
}
