using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Weapsy.Data.Providers
{
    public class SqlServerDatabaseProvider : IDatabaseProvider
    {
        public ProviderType ProviderType { get; } = ProviderType.SqlServer;

        public IServiceCollection RegisterDbContext(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<WeapsyDbContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }

        public WeapsyDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<WeapsyDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new WeapsyDbContext(optionsBuilder.Options);
        }
    }
}
