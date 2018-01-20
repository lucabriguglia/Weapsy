using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Weapsy.Data
{
    public class MigrationsDbContextFactory : IDesignTimeDbContextFactory<WeapsyDbContext>
    {
        public WeapsyDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<WeapsyDbContext>();
            builder.UseSqlServer("UsedForMigrationsOnlyUntilClassLibraryBugIsFixed");

            return new WeapsyDbContext(builder.Options);
        }
    }
}