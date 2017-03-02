using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Weapsy.Data
{
    public class MigrationsDbContextFactory : IDbContextFactory<WeapsyDbContext>
    {
        public WeapsyDbContext Create(DbContextFactoryOptions options)
        {
            var builder = new DbContextOptionsBuilder<WeapsyDbContext>();
            builder.UseSqlServer("UsedForMigrationsOnlyUntilClassLibraryBugIsFixed");

            return new WeapsyDbContext(builder.Options);
        }
    }
}