using Microsoft.EntityFrameworkCore;
using Weapsy.Domain.Models.Sites;

namespace Weapsy.Data
{
    public class WeapsyDbContext : DbContext
    {
        public WeapsyDbContext(DbContextOptions<WeapsyDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Site>()
                .ToTable("Site");
        }

        public DbSet<Site> Sites { get; set; }
    }
}
