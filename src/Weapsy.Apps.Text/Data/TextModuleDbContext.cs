using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Weapsy.Apps.Text.Data.Entities;
using Weapsy.Infrastructure.Configuration;

namespace Weapsy.Apps.Text.Data
{
    public class TextModuleDbContext : DbContext
    {
        private ConnectionStrings ConnectionStrings { get; set; }

        public TextModuleDbContext(IOptions<ConnectionStrings> settings)
        {
            ConnectionStrings = settings.Value;
        }

        public TextModuleDbContext(DbContextOptions<TextModuleDbContext> options) : base(options) {}

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
    }
}
