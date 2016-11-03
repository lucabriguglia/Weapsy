using System;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Pages
{
    public class PageModulePermission : ValueObject
    {
        public Guid PageModuleId { get; set; }  
        public PermissionType Type { get; set; }
        public string RoleId { get; set; }
    }
}
