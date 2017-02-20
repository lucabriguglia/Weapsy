using Microsoft.AspNetCore.Identity;
using Weapsy.Data.Entities;
using Weapsy.Infrastructure.Identity;

namespace Weapsy.Services.Installation
{
    public class MembershipInstallationService : IMembershipInstallationService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public MembershipInstallationService(UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async void VerifyUserCreation()
        {
            var adminEmail = "admin@default.com";
            var adminUser = new User { UserName = adminEmail, Email = adminEmail };

            var adminRole = new Role { Id = Administrator.Id, Name = Administrator.Name };

            if (await _userManager.FindByEmailAsync(adminEmail) == null)
            {
                await _userManager.CreateAsync(adminUser, "Ab1234567!");
            }

            if (!await _roleManager.RoleExistsAsync(adminRole.Name))
            {
                await _roleManager.CreateAsync(adminRole);
            }

            adminUser = await _userManager.FindByEmailAsync(adminEmail);

            if (!await _userManager.IsInRoleAsync(adminUser, adminRole.Name))
            {
                await _userManager.AddToRoleAsync(adminUser, adminRole.Name);
            }
        }
    }
}
