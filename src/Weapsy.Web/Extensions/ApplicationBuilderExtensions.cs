using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Weapsy.Data;
using Weapsy.Data.TempIdentity;
using Weapsy.Domain.Roles.DefaultRoles;

namespace Weapsy.Web.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder EnsureDbCreated(this IApplicationBuilder app)
        {
            var dbContext = app.ApplicationServices.GetRequiredService<WeapsyDbContext>();

            dbContext.Database.Migrate();

            return app;
        }

        public static async Task<IApplicationBuilder> EnsureIdentityCreatedAsync(this IApplicationBuilder app, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            //var userManager = app.ApplicationServices.GetRequiredService<UserManager<ApplicationUser>>();
            //var roleManager = app.ApplicationServices.GetRequiredService<RoleManager<ApplicationRole>>();

            if (!await roleManager.RoleExistsAsync(Administrator.Name)) {
                await roleManager.CreateAsync(new ApplicationRole
                {
                    Id = Administrator.Id,
                    Name = Administrator.Name
                });
            }

            var adminEmail = "admin@default.com";

            if (await userManager.FindByEmailAsync(adminEmail) == null) {
                var user = new ApplicationUser { UserName = adminEmail, Email = adminEmail };
                await userManager.CreateAsync(user, "Ab1234567!");
            }

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (!await userManager.IsInRoleAsync(adminUser, Administrator.Name)) {
                await userManager.AddToRoleAsync(adminUser, Administrator.Name);
            }

            return app;
        }
    }
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
