using System;
using Microsoft.AspNetCore.Identity;

namespace Weapsy.Data.TempIdentity
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser<Guid>
    {
    }
}
