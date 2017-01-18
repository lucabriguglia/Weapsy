using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Weapsy.Infrastructure.Configuration;

namespace Weapsy.Data
{
    public class WeapsyDbContextFactory : IWeapsyDbContextFactory
    {
        private ConnectionStrings ConnectionStrings { get; }

        public WeapsyDbContextFactory(IOptions<ConnectionStrings> settings)
        {
            ConnectionStrings = settings.Value;
        }

        public BaseDbContext Create()
        {
            var optionsBuilder = new DbContextOptionsBuilder<WeapsyDbContext>();
            optionsBuilder.UseSqlServer(ConnectionStrings.DefaultConnection);

            return new WeapsyDbContext(optionsBuilder.Options);
        }

        public BaseDbContext Create(DbContextOptions options)
        {
            return new WeapsyDbContext(options);
        }
    }
}
