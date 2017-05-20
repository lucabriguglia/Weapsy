using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Weapsy.Framework.Configuration;

namespace Weapsy.Apps.Text.Data.Providers
{
    public class SQLiteDataProvider : IDataProvider
    {
        public DataProvider Provider { get; } = DataProvider.SQLite;

        public IServiceCollection RegisterDbContext(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<TextDbContext>(options =>
                options.UseSqlite(connectionString));

            return services;
        }

        public TextDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TextDbContext>();
            optionsBuilder.UseSqlite(connectionString);

            return new TextDbContext(optionsBuilder.Options);
        }
    }
}