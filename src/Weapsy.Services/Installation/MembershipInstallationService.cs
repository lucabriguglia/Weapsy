using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Weapsy.Services.Installation
{
    public class MembershipInstallationService : IMembershipInstallationService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public MembershipInstallationService(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async void VerifyUserCreation()
        {
            var adminEmail = "admin@default.com";
            var adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail };

            var adminRoleName = "Administrator";
            var adminRole = new IdentityRole(adminRoleName);

            if (await _userManager.FindByEmailAsync(adminEmail) == null)
            {
                await _userManager.CreateAsync(adminUser, "Ab1234567!");
            }

            if (!await _roleManager.RoleExistsAsync(adminRoleName))
            {
                await _roleManager.CreateAsync(adminRole);
            }

            adminUser = await _userManager.FindByEmailAsync(adminEmail);

            if (!await _userManager.IsInRoleAsync(adminUser, adminRoleName))
            {
                await _userManager.AddToRoleAsync(adminUser, adminRoleName);
            }
        }
    }
}
