using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Weapsy.Data.Entities;
using Weapsy.Infrastructure.Configuration;

namespace Weapsy.Data.Providers.PostgreSQL
{
    public class PostgreSQLDbContext : WeapsyDbContext
    {
        private ConnectionStrings ConnectionStrings { get; }

        public PostgreSQLDbContext(IOptions<ConnectionStrings> settings)
        {
            ConnectionStrings = settings.Value;
        }

        public PostgreSQLDbContext(DbContextOptions options)
            : base(options)
        {            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!string.IsNullOrEmpty(ConnectionStrings?.DefaultConnection))
                builder.UseNpgsql(ConnectionStrings.DefaultConnection);

            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<App>()
                .ToTable("App");

            builder.Entity<EmailAccount>()
                .ToTable("EmailAccount");

            builder.Entity<Language>()
                .ToTable("Language");

            builder.Entity<Menu>()
                .ToTable("Menu");

            builder.Entity<MenuItem>()
                .ToTable("MenuItem");

            builder.Entity<MenuItemLocalisation>()
                .ToTable("MenuItemLocalisation")
                .HasKey(x => new { x.MenuItemId, x.LanguageId });

            builder.Entity<MenuItemPermission>()
                .ToTable("MenuItemPermission")
                .HasKey(x => new { x.MenuItemId, x.RoleId });

            builder.Entity<Module>()
                .ToTable("Module");

            builder.Entity<ModuleType>()
                .ToTable("ModuleType");

            builder.Entity<Page>()
                .ToTable("Page");

            builder.Entity<PageLocalisation>()
                .ToTable("PageLocalisation")
                .HasKey(x => new { x.PageId, x.LanguageId });

            builder.Entity<PageModule>()
                .ToTable("PageModule");

            builder.Entity<PageModuleLocalisation>()
                .ToTable("PageModuleLocalisation")
                .HasKey(x => new { x.PageModuleId, x.LanguageId });

            builder.Entity<PageModulePermission>()
                .ToTable("PageModulePermission")
                .HasKey(x => new { x.PageModuleId, x.RoleId, x.Type });

            builder.Entity<PagePermission>()
                .ToTable("PagePermission")
                .HasKey(x => new { x.PageId, x.RoleId, x.Type });

            builder.Entity<Site>()
                .ToTable("Site");

            builder.Entity<SiteLocalisation>()
                .ToTable("SiteLocalisation")
                .HasKey(x => new { x.SiteId, x.LanguageId });

            builder.Entity<Theme>()
                .ToTable("Theme");

            builder.Entity<User>()
                .ToTable("User");
        }
    }
}
