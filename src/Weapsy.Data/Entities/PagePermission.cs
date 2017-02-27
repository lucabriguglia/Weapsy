using System;
using Weapsy.Domain.Pages;

namespace Weapsy.Data.Entities
{
    public class PagePermission
    {
        public Guid PageId { get; set; }  
        public PermissionType Type { get; set; }
        public Guid RoleId { get; set; }

        public virtual Page Page { get; set; }
    }
}
