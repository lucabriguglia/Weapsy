using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Weapsy.Infrastructure.Configuration;

namespace Weapsy.Data.Providers.PostgreSQL
{
    public class PostgreSQLDataProvider : IDataProvider
    {
        public DataProvider Provider { get; } = DataProvider.PostgreSQL;
        private ConnectionStrings ConnectionStrings { get; }

        public PostgreSQLDataProvider(IOptions<ConnectionStrings> settings)
        {
            ConnectionStrings = settings.Value;
        }

        public WeapsyDbContext DbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<PostgreSQLDbContext>();
            optionsBuilder.UseNpgsql(ConnectionStrings.DefaultConnection);

            return new PostgreSQLDbContext(optionsBuilder.Options);
        }
    }
}