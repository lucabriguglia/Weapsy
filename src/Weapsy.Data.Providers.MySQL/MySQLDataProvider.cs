using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MySQL.Data.EntityFrameworkCore.Extensions;
using Weapsy.Infrastructure.Configuration;

namespace Weapsy.Data.Providers.MySQL
{
    public class MySQLDataProvider : IDataProvider
    {
        public DataProvider Provider { get; } = DataProvider.MySQL;
        private ConnectionStrings ConnectionStrings { get; }

        public MySQLDataProvider(IOptions<ConnectionStrings> settings)
        {
            ConnectionStrings = settings.Value;
        }

        public WeapsyDbContext DbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MySQLDbContext>();
            optionsBuilder.UseMySQL(ConnectionStrings.DefaultConnection);

            return new MySQLDbContext(optionsBuilder.Options);
        }
    }
}