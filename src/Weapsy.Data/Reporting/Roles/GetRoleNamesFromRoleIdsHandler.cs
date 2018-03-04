using System.Threading.Tasks;
using Weapsy.Reporting.Roles.Queries;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Weapsy.Domain.Roles.DefaultRoles;
using Weapsy.Cqrs.Queries;
using Weapsy.Data.TempIdentity;

namespace Weapsy.Data.Reporting.Roles
{
    public class GetRoleNamesFromRoleIdsHandler : IQueryHandlerAsync<GetRoleNamesFromRoleIds, IEnumerable<string>>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public GetRoleNamesFromRoleIdsHandler(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
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
