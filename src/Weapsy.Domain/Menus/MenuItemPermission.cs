using System;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Menus
{
    public class MenuItemPermission : ValueObject
    {
        public Guid MenuItemId { get; set; }  
        public Guid RoleId { get; set; }
    }
}
