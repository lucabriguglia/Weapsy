using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Weapsy.Framework.Configuration;

namespace Weapsy.Data.Providers
{
    public class PostgreSQLDataProvider : IDataProvider
    {
        public DataProvider Provider { get; } = DataProvider.PostgreSQL;

        public IServiceCollection RegisterDbContext(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<WeapsyDbContext>(options =>
                options.UseNpgsql(connectionString));

            return services;
        }

        public WeapsyDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<WeapsyDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new WeapsyDbContext(optionsBuilder.Options);
        }
    }
}