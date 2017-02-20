using System;
using System.Collections.Generic;

namespace Weapsy.Domain.Pages.Commands
{
    public class AddModule : BaseSiteCommand
    {
        public Guid PageId { get; set; }
        public Guid ModuleTypeId { get; set; }
        public string Zone { get; set; }
        public int SortOrder { get; set; }
        public string Title { get; set; }
        public IList<PageModulePermission> PageModulePermissions { get; set; } = new List<PageModulePermission>();

        public void SetPageModulePermissions(IList<Guid> viewRoleIds, IList<Guid> editRoleIds)
        {
            foreach (var viewRoleId in viewRoleIds)
                PageModulePermissions.Add(new PageModulePermission
                {
                    RoleId = viewRoleId,
                    Type = PermissionType.View
                });

            foreach (var viewRoleId in editRoleIds)
                PageModulePermissions.Add(new PageModulePermission
                {
                    RoleId = viewRoleId,
                    Type = PermissionType.Edit
                });
        }
    }
}
