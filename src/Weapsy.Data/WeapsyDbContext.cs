using Microsoft.EntityFrameworkCore;
using Weapsy.Data.Entities;

namespace Weapsy.Data
{
    public abstract class WeapsyDbContext : DbContext
    {
        protected WeapsyDbContext()
        {
        }

        protected WeapsyDbContext(DbContextOptions options)
            : base(options)
        {            
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
