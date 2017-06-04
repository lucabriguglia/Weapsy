using AutoMapper;
using System.Threading.Tasks;
using Weapsy.Reporting.Roles.Queries;
using System.Collections.Generic;
using Weapsy.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Weapsy.Domain.Roles.DefaultRoles;
using Weapsy.Framework.Queries;

namespace Weapsy.Data.Reporting.Roles
{
    public class GetRoleNamesFromRoleIdsHandler : IQueryHandlerAsync<GetRoleNamesFromRoleIds, IEnumerable<string>>
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public GetRoleNamesFromRoleIdsHandler(RoleManager<Role> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<string>> RetrieveAsync(GetRoleNamesFromRoleIds query)
        {
            var result = new List<string>();

            foreach (var roleId in query.RoleIds)
            {
                if (Everyone.Id == roleId)
                {
                    result.Add(Everyone.Name);
                }
                else if (Registered.Id == roleId)
                {
                    result.Add(Registered.Name);
                }
                else if (Anonymous.Id == roleId)
                {
                    result.Add(Anonymous.Name);
                }
                else
                {
                    var role = await _roleManager.FindByIdAsync(roleId.ToString());

                    if (role != null)
                    {
                        result.Add(role.Name);
                    }
                }
            }

            return result;
        }
    }
}
