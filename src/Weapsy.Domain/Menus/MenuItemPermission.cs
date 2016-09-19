using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Menus
{
    public class MenuItemPermission : ValueObject
    {
        public Guid MenuItemId { get; set; }  
        public string RoleId { get; set; }
    }
}
