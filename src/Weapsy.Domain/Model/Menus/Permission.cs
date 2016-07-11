using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Menus
{
    public class ViewPermission : ValueObject
    {
        public Guid MenuItemId { get; set; }  
        public string RoleId { get; set; }
    }
}
