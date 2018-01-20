using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Weapsy.Apps.Text.Data
{
    public class MigrationsDbContextFactory : IDesignTimeDbContextFactory<TextDbContext>
    {
        public TextDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<TextDbContext>();
            builder.UseSqlServer("UsedForMigrationsOnlyUntilClassLibraryBugIsFixed");

            return new TextDbContext(builder.Options);
        }
    }
}