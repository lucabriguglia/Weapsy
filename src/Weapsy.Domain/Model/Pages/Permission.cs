using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Pages
{
    public class Permission : ValueObject
    {
        public Guid PageId { get; set; }  
        public PermissionType Type { get; set; }
        public string RoleId { get; set; }
    }
}
