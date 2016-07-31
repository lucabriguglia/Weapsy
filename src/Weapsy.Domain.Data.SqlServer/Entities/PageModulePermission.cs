using System;
using Weapsy.Domain.Model.Pages;

namespace Weapsy.Domain.Data.SqlServer.Entities
{
    public class PageModulePermission : IDbEntity
    {
        public Guid PageModuleId { get; set; }  
        public PermissionType Type { get; set; }
        public string RoleId { get; set; }
    }
}
