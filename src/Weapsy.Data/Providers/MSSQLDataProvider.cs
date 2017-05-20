using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Weapsy.Framework.Configuration;

namespace Weapsy.Data.Providers
{
    public class MSSQLDataProvider : IDataProvider
    {
        public DataProvider Provider { get; } = DataProvider.MSSQL;

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