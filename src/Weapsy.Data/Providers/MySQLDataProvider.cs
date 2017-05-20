using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySQL.Data.EntityFrameworkCore.Extensions;
using Weapsy.Framework.Configuration;

namespace Weapsy.Data.Providers
{
    public class MySQLDataProvider : IDataProvider
    {
        public DataProvider Provider { get; } = DataProvider.MySQL;

        public IServiceCollection RegisterDbContext(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<WeapsyDbContext>(options => 
                options.UseMySQL(connectionString));

            return services;
        }

        public WeapsyDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<WeapsyDbContext>();
            optionsBuilder.UseMySQL(connectionString);

            return new WeapsyDbContext(optionsBuilder.Options);
        }
    }
}