using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using Weapsy.Core.Configuration;
using Weapsy.Domain.Data.Entities;

namespace Weapsy.Domain.Data
{
    public class WeapsyDbContext : DbContext
    {
        private ConnectionStrings ConnectionStrings { get; set; }

        public WeapsyDbContext(IOptions<ConnectionStrings> settings)
        {
            ConnectionStrings = settings.Value;
        }

        public WeapsyDbContext(DbContextOptions<WeapsyDbContext> options) : base(options) {}

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (ConnectionStrings != null && !string.IsNullOrEmpty(ConnectionStrings.DefaultConnection))
                builder.UseSqlServer(ConnectionStrings.DefaultConnection);
            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<App>()
                .ToTable("App");

            builder.Entity<Language>()
                .ToTable("Language");

            builder.Entity<Menu>()
                .ToTable("Menu");

            builder.Entity<MenuItem>()
                .ToTable("MenuItem");

            builder.Entity<MenuItemLocalisation>()
                .ToTable("MenuItemLocalisation")
                .HasKey(x => new { x.MenuItemId, x.LanguageId });

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

            builder.Entity<Site>()
                .ToTable("Site");

            builder.Entity<SiteLocalisation>()
                .ToTable("SiteLocalisation")
                .HasKey(x => new { x.SiteId, x.LanguageId });

            builder.Entity<Theme>()
                .ToTable("Theme");

            //var assembly = Assembly.GetExecutingAssembly();

            //var allTypes = assembly.GetTypes()
            //    .Where(t => t.BaseType != null && t.BaseType.IsGenericType).ToArray();

            //var configTypes = allTypes.Where(t => t.BaseType != null && t.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeBuilder<>));

            //foreach (var type in configTypes)
            //{
            //    dynamic configurationInstance = Activator.CreateInstance(type);
            //    builder.Configurations.Add(configurationInstance);
            //}

            //var entityTypes = allTypes.Where(t => t.IsClass && t.GetInterface(typeof(IEntity).FullName) != null);

            //foreach (var type in entityTypes)
            //{
            //    MethodInfo method = builder.GetType().GetMethod("Entity");
            //    method = method.MakeGenericMethod(type);
            //    method.Invoke(builder, null);
            //}
        }

        //public override int SaveChanges()
        //{
        //    var domainEventEntities = ChangeTracker.Entries<IEntity>()
        //        .Select(po => po.Entity)
        //        .Where(po => po.Events.Any())
        //        .ToArray();

        //    foreach (var entity in domainEventEntities)
        //    {
        //        var events = entity.Events.ToArray();
        //        entity.Events.Clear();
        //        foreach (var domainEvent in events)
        //        {
        //            _dispatcher.Dispatch(domainEvent);
        //        }
        //    }

        //    return base.SaveChanges();
        //}

        public new DbSet<T> Set<T>() where T : class, IDbEntity
        {
            return base.Set<T>();
        }

        public new EntityEntry<T> Entry<T>(T entity) where T : class, IDbEntity
        {
            return base.Entry<T>(entity);
        }
    }
}
