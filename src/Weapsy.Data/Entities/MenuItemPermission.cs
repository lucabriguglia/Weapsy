using System;

namespace Weapsy.Data.Entities
{
    public class MenuItemPermission : IDbEntity
    {
        public Guid MenuItemId { get; set; }  
        public string RoleId { get; set; }

        public virtual MenuItem MenuItem { get; set; }
    }
}
