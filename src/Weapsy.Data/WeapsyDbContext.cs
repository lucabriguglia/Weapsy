using Microsoft.EntityFrameworkCore;
using Weapsy.Data.Entities;

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

            builder.Entity<SiteEntity>()
                .ToTable("Site");
        }

        public DbSet<SiteEntity> Sites { get; set; }
    }
}
