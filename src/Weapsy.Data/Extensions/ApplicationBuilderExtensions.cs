using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Weapsy.Data.Entities;
using Weapsy.Framework.Identity;

namespace Weapsy.Data.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder EnsureDbCreated(this IApplicationBuilder app)
        {
            var dbContext = app.ApplicationServices.GetRequiredService<WeapsyDbContext>();

            dbContext.Database.Migrate();

            return app;
        }

        public static async Task<IApplicationBuilder> EnsureIdentityCreated(this IApplicationBuilder app)
        {
            var userManager = app.ApplicationServices.GetRequiredService<UserManager<User>>();
            var roleManager = app.ApplicationServices.GetRequiredService<RoleManager<Role>>();

            if (!await roleManager.RoleExistsAsync(Administrator.Name))
            {
                await roleManager.CreateAsync(new Role
                {
                    Id = Administrator.Id,
                    Name = Administrator.Name
                });
            }

            var adminEmail = "admin@default.com";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var user = new User { UserName = adminEmail, Email = adminEmail };
                await userManager.CreateAsync(user, "Ab1234567!");
            }

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (!await userManager.IsInRoleAsync(adminUser, Administrator.Name))
            {
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
