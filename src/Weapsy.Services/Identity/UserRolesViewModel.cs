using System.Collections.Generic;
using Weapsy.Data.Entities;

namespace Weapsy.Services.Identity
{
    public class UserRolesViewModel
    {
        public User User { get; set; }
        public IList<Role> AvailableRoles { get; set; }
        public IList<string> UserRoles { get; set; }
    }
}
