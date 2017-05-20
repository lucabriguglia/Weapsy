using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySQL.Data.EntityFrameworkCore.Extensions;
using Weapsy.Framework.Configuration;

namespace Weapsy.Apps.Text.Data.Providers
{
    public class MySQLDataProvider : IDataProvider
    {
        public DataProvider Provider { get; } = DataProvider.MySQL;

        public IServiceCollection RegisterDbContext(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<TextDbContext>(options => 
                options.UseMySQL(connectionString));

            return services;
        }

        public TextDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TextDbContext>();
            optionsBuilder.UseMySQL(connectionString);

            return new TextDbContext(optionsBuilder.Options);
        }
    }
}