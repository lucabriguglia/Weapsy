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

        public async void EnsureIdentityInstalled()
        {
            EnsureDefaultRolesCreated();

            var adminEmail = "admin@default.com";
            var adminUser = new User { UserName = adminEmail, Email = adminEmail };

            if (await _userManager.FindByEmailAsync(adminEmail) == null)
            {
                await _userManager.CreateAsync(adminUser, "Ab1234567!");
            }

            adminUser = await _userManager.FindByEmailAsync(adminEmail);

            if (!await _userManager.IsInRoleAsync(adminUser, Administrator.Name))
            {
                await _userManager.AddToRoleAsync(adminUser, Administrator.Name);
            }
        }

        private async void EnsureDefaultRolesCreated()
        {
            if (!await _roleManager.RoleExistsAsync(Administrator.Name))
            {
                await _roleManager.CreateAsync(new Role
                {
                    Id = Administrator.Id,
                    Name = Administrator.Name
                });
            }

            //if (!await _roleManager.RoleExistsAsync(Everyone.Name))
            //{
            //    await _roleManager.CreateAsync(new Role
            //    {
            //        Id = Everyone.Id,
            //        Name = Everyone.Name
            //    });
            //}

            //if (!await _roleManager.RoleExistsAsync(Registered.Name))
            //{
            //    await _roleManager.CreateAsync(new Role
            //    {
            //        Id = Registered.Id,
            //        Name = Registered.Name
            //    });
            //}

            //if (!await _roleManager.RoleExistsAsync(Anonymous.Name))
            //{
            //    await _roleManager.CreateAsync(new Role
            //    {
            //        Id = Anonymous.Id,
            //        Name = Anonymous.Name
            //    });
            //}
        }
    }
}
