using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Pages
{
    public class Permission : ValueObject
    {
        public PermissionType PermissionType { get; set; }
        public string RoleName { get; set; }
    }
}
