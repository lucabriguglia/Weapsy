using System;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Pages
{
    public class PagePermission : ValueObject
    {
        public Guid PageId { get; set; }  
        public PermissionType Type { get; set; }
        public Guid RoleId { get; set; }
    }
}
