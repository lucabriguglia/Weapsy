using System;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Pages
{
    public class PageModulePermission : ValueObject
    {
        public Guid PageModuleId { get; set; }  
        public PermissionType Type { get; set; }
        public Guid RoleId { get; set; }
    }
}
