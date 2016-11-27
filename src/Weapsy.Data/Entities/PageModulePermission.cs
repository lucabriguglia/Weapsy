using System;
using Weapsy.Domain.Pages;

namespace Weapsy.Data.Entities
{
    public class PageModulePermission : IDbEntity
    {
        public Guid PageModuleId { get; set; }  
        public PermissionType Type { get; set; }
        public string RoleId { get; set; }
    }
}
