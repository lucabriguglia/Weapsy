using System;
using System.Collections.Generic;
using System.Linq;
using Weapsy.Domain.Pages;

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
                    if (!permissionType.Selected && !permission.Disabled)
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
                    if(!permissionType.Selected && !permission.Disabled)
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

        public static List<Guid> ToCommand(this IList<MenuModel> menus)
        {
            return (from menu in menus where menu.Selected select menu.MenuId).ToList();
        }
    }
}
