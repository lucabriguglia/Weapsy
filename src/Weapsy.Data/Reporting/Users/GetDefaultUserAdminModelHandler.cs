using AutoMapper;
using Weapsy.Infrastructure.Queries;
using System.Threading.Tasks;
using Weapsy.Reporting.Users.Queries;
using Weapsy.Reporting.Users;

namespace Weapsy.Data.Reporting.Users
{
    public class GetDefaultUserAdminModelHandler : IQueryHandlerAsync<GetDefaultUserAdminModel, UserAdminModel>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetDefaultUserAdminModelHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<UserAdminModel> RetrieveAsync(GetDefaultUserAdminModel query)
        {
            return new UserAdminModel();
        }
    }
}
