using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Weapsy.Infrastructure.Configuration;

namespace Weapsy.Data.Providers.MSSQL
{
    public class MSSQLDataProvider : IDataProvider
    {
        public DataProvider Provider { get; } = DataProvider.MSSQL;
        private ConnectionStrings ConnectionStrings { get; }

        public MSSQLDataProvider(IOptions<ConnectionStrings> settings)
        {
            ConnectionStrings = settings.Value;
        }

        public WeapsyDbContext DbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MSSQLDbContext>();
            optionsBuilder.UseSqlServer(ConnectionStrings.DefaultConnection);

            return new MSSQLDbContext(optionsBuilder.Options);
        }
    }
}