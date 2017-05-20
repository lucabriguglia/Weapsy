using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Weapsy.Framework.Configuration;

namespace Weapsy.Apps.Text.Data.Providers
{
    public class PostgreSQLDataProvider : IDataProvider
    {
        public DataProvider Provider { get; } = DataProvider.PostgreSQL;

        public IServiceCollection RegisterDbContext(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<TextDbContext>(options =>
                options.UseNpgsql(connectionString));

            return services;
        }

        public TextDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TextDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new TextDbContext(optionsBuilder.Options);
        }
    }
}