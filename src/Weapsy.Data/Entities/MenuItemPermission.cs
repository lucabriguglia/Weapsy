using System;

namespace Weapsy.Data.Entities
{
    public class MenuItemPermission
    {
        public Guid MenuItemId { get; set; }  
        public Guid RoleId { get; set; }

        public virtual MenuItem MenuItem { get; set; }
    }
}
