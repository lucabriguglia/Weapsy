using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Weapsy.Core.Configuration;

namespace Weapsy.Domain.Data.SqlServer
{
    public class WeapsyDbContextFactory : IWeapsyDbContextFactory
    {
        private ConnectionStrings ConnectionStrings { get; }

        public WeapsyDbContextFactory(IOptions<ConnectionStrings> settings)
        {
            ConnectionStrings = settings.Value;
        }

        public WeapsyDbContext Create()
        {
            var optionsBuilder = new DbContextOptionsBuilder<WeapsyDbContext>();
            optionsBuilder.UseSqlServer(ConnectionStrings.DefaultConnection);

            return new WeapsyDbContext(optionsBuilder.Options);
        }

        public WeapsyDbContext Create(DbContextOptions<WeapsyDbContext> options)
        {
            return new WeapsyDbContext(options);
        }
    }
}
