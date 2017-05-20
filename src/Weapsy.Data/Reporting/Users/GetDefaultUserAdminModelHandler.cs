using AutoMapper;
using System.Threading.Tasks;
using Weapsy.Reporting.Users.Queries;
using Weapsy.Reporting.Users;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Weapsy.Framework.Queries;

namespace Weapsy.Data.Reporting.Users
{
    public class GetDefaultUserAdminModelHandler : IQueryHandlerAsync<GetDefaultUserAdminModel, UserAdminModel>
    {
        private readonly IContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetDefaultUserAdminModelHandler(IContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<UserAdminModel> RetrieveAsync(GetDefaultUserAdminModel query)
        {
            using (var context = _contextFactory.Create())
            {
                var model = new UserAdminModel
                {
                    AllRoles = await context.Roles.Select(x => x.Name).ToListAsync()
                };

                return model;
            }

        }
    }
}
