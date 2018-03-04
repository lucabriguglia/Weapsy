using AutoMapper;
using System.Threading.Tasks;
using Weapsy.Reporting.Roles.Queries;
using System.Collections.Generic;
using Weapsy.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Weapsy.Data.TempIdentity;
using Weapsy.Domain.Roles.DefaultRoles;
using Weapsy.Cqrs.Queries;

namespace Weapsy.Data.Reporting.Roles
{
    public class GetAllRolesHandler : IQueryHandlerAsync<GetAllRoles, IEnumerable<ApplicationRole>>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IMapper _mapper;

        public GetAllRolesHandler(RoleManager<ApplicationRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ApplicationRole>> RetrieveAsync(GetAllRoles query)
        {
            var result = _roleManager.Roles.ToList();

            result.Add(new ApplicationRole
            {
                Id = Everyone.Id,
                Name = Everyone.Name
            });

            result.Add(new ApplicationRole
            {
                Id = Registered.Id,
                Name = Registered.Name
            });

            result.Add(new ApplicationRole
            {
                Id = Anonymous.Id,
                Name = Anonymous.Name
            });

            return result.OrderBy(x => x.Name).ToList();
        }
    }
}
