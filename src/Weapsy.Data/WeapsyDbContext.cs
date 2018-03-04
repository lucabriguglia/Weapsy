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

        public DbSet<App> Apps { get; set; }
        public DbSet<EmailAccount> EmailAccounts { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<MenuItemLocalisation> MenuItemLocalisations { get; set; }
        public DbSet<MenuItemPermission> MenuItemPermissions { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<ModuleType> ModuleTypes { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<PageLocalisation> PageLocalisations { get; set; }
        public DbSet<PageModule> PageModules { get; set; }
        public DbSet<PageModuleLocalisation> PageModuleLocalisations { get; set; }
        public DbSet<PageModulePermission> PageModulePermissions { get; set; }
        public DbSet<PagePermission> PagePermissions { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<SiteLocalisation> SiteLocalisations { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
