using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Weapsy.Infrastructure.Configuration;

namespace Weapsy.Data.Providers.SQLite
{
    public class SQLiteDataProvider : IDataProvider
    {
        public DataProvider Provider { get; } = DataProvider.SQLite;
        private ConnectionStrings ConnectionStrings { get; }

        public SQLiteDataProvider(IOptions<ConnectionStrings> settings)
        {
            ConnectionStrings = settings.Value;
        }

        public WeapsyDbContext DbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<SQLiteDbContext>();
            optionsBuilder.UseSqlite(ConnectionStrings.DefaultConnection);

            return new SQLiteDbContext(optionsBuilder.Options);
        }
    }
}