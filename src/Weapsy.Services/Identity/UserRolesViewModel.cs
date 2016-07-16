using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;

namespace Weapsy.Services.Identity
{
    public class UserRolesViewModel
    {
        public IdentityUser User { get; set; }
        public IList<IdentityRole> AvailableRoles { get; set; }
        public IList<string> UserRoles { get; set; }
    }
}
