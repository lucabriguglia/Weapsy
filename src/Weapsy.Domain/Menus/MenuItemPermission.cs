using System;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Menus
{
    public class MenuItemPermission : ValueObject
    {
        public Guid MenuItemId { get; set; }  
        public Guid RoleId { get; set; }
    }
}
