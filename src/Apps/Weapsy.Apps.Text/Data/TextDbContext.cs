using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Weapsy.Apps.Text.Data.Entities;
using Weapsy.Framework.Configuration;

namespace Weapsy.Apps.Text.Data
{
    public class TextDbContext : DbContext
    {
        private ConnectionStrings ConnectionStrings { get; set; }

        public TextDbContext(IOptions<ConnectionStrings> settings)
        {
            ConnectionStrings = settings.Value;
        }

        public TextDbContext(DbContextOptions<TextDbContext> options) : base(options) {}

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (ConnectionStrings != null && !string.IsNullOrEmpty(ConnectionStrings.DefaultConnection))
                builder.UseSqlServer(ConnectionStrings.DefaultConnection);

            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TextModule>()
                .ToTable("TextModule");

            builder.Entity<TextVersion>()
                .ToTable("TextVersion");

            builder.Entity<TextLocalisation>()
                .ToTable("TextLocalisation")
                .HasKey(x => new { x.TextVersionId, x.LanguageId });
        }

        public DbSet<TextModule> TextModules { get; set; }
        public DbSet<TextVersion> TextVersions { get; set; }
        public DbSet<TextLocalisation> TextLocalisations { get; set; }
    }
}
