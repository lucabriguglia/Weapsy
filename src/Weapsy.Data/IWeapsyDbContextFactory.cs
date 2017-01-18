using Microsoft.EntityFrameworkCore;

namespace Weapsy.Data
{
    public interface IWeapsyDbContextFactory
    {
        BaseDbContext Create();
        BaseDbContext Create(DbContextOptions options);
    }
}
