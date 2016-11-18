using System;
using System.Collections.Generic;
using System.Linq;
using Weapsy.Domain.Pages;
using Weapsy.Services.Identity;

namespace Weapsy.Reporting.Pages
{
    public static class Extensions
    {
        public static List<PagePermission> ToDomain(this IList<PagePermissionModel> models)
        {
            var result = new List<PagePermission>();

            foreach (var permission in models)
            {
                foreach (var permissionType in permission.PagePermissionTypes)
                {
                    if (!permissionType.Selected)
                        continue;

                    result.Add(new PagePermission
                    {
                        PageId = permission.PageId,
                        RoleId = permission.RoleId,
                        Type = permissionType.Type
                    });
                }
            }

            return result;
        }

        public static List<PageModulePermission> ToDomain(this IList<PageModulePermissionModel> models)
        {
            var result = new List<PageModulePermission>();

            foreach (var permission in models)
            {
                foreach (var permissionType in permission.PageModulePermissionTypes)
                {
                    if(!permissionType.Selected)
                        continue;

                    result.Add(new PageModulePermission
                    {
                        PageModuleId = permission.PageModuleId,
                        RoleId = permission.RoleId,
                        Type = permissionType.Type
                    });                    
                }
            }

            return result;
        }

        //public static Dictionary<PermissionType, IEnumerable<string>> ToRoleDictionary(this IList<PagePermission> pagePermissions, IRoleService roleService)
        //{
        //    var result = new Dictionary<PermissionType, IEnumerable<string>>();

        //    foreach (PermissionType permisisonType in Enum.GetValues(typeof(PermissionType)))
        //    {
        //        var pageRoleIds = pagePermissions.Where(x => x.Type == permisisonType).Select(x => x.RoleId);
        //        var pageRoles = roleService.GetRolesFromIds(pageRoleIds);
        //        result.Add(permisisonType, pageRoles.Select(x => x.Name));
        //    }

        //    return result;
        //}
    }
}
