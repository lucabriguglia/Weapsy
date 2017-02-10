using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Weapsy.Data.Identity
{
    public class UserRolesViewModel
    {
        public IdentityUser User { get; set; }
        public IList<IdentityRole> AvailableRoles { get; set; }
        public IList<string> UserRoles { get; set; }
    }
}
