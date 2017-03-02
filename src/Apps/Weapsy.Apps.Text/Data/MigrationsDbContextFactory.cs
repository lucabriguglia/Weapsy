using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Weapsy.Apps.Text.Data
{
    public class MigrationsDbContextFactory : IDbContextFactory<TextDbContext>
    {
        public TextDbContext Create(DbContextFactoryOptions options)
        {
            var builder = new DbContextOptionsBuilder<TextDbContext>();
            builder.UseSqlServer("UsedForMigrationsOnlyUntilClassLibraryBugIsFixed");

            return new TextDbContext(builder.Options);
        }
    }
}